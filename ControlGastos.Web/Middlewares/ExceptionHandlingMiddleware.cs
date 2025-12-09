using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace ControlGastos.Web.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var errors = new List<string>();

            switch (exception)
            {
                // 1) Errores de validación (FluentValidation)
                case ValidationException validationEx:
                    statusCode = (int)HttpStatusCode.BadRequest;

                    var validationErrors = validationEx.Errors?
                        .Select(e => e.ErrorMessage)
                        .Where(m => !string.IsNullOrWhiteSpace(m))
                        .ToList() ?? new List<string>();

                    if (validationErrors.Count == 0 && !string.IsNullOrWhiteSpace(validationEx.Message))
                        validationErrors.Add(validationEx.Message);

                    errors = validationErrors;
                    break;
                // 1.b) Login / credenciales inválidas
                case ControlGastos.Domain.Exceptions.InvalidCredentialsException invalidCredsEx:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    errors.Add(invalidCredsEx.Message);
                    break;

                // 2) Errores de base de datos (FK, UNIQUE, etc.)
                case DbUpdateException dbEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errors = MapDbErrorToUserMessages(dbEx);
                    break;

                // 3) Recurso no encontrado (lo usamos para metas, usuarios, etc.)
                case KeyNotFoundException notFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    errors.Add(notFoundEx.Message);
                    break;

                // 4) No autorizado
                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    errors.Add("No tenés autorización para realizar esta acción.");
                    break;

                case ControlGastos.Domain.Exceptions.ForbiddenAccessException forbiddenEx:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    errors.Add(forbiddenEx.Message);
                    break;

                // 5) Cualquier otra cosa
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errors.Add("Ocurrió un error inesperado. Si el problema persiste, contactá al administrador.");
                    break;
            }

            // Log completo para debugging
            _logger.LogError(exception, "Error no controlado: {Message}", exception.Message);

            var errorResponse = new ErrorResponse
            {
                StatusCode = statusCode,
                Messages = errors,
                TraceId = context.TraceIdentifier
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }

        /// <summary>
        /// Traduce errores de base de datos (FK, UNIQUE, etc.) a mensajes claros para el front.
        /// </summary>
        private static List<string> MapDbErrorToUserMessages(DbUpdateException dbEx)
        {
            var messages = new List<string>();

            var rawMessage =
                dbEx.InnerException?.InnerException?.Message ??
                dbEx.InnerException?.Message ??
                dbEx.Message;

            if (string.IsNullOrWhiteSpace(rawMessage))
            {
                messages.Add("Ocurrió un error al guardar los cambios en la base de datos.");
                return messages;
            }

            var normalized = rawMessage.ToUpperInvariant();

            // Conflictos de FOREIGN KEY
            if (normalized.Contains("FOREIGN KEY"))
            {
                if (normalized.Contains("FK_PRESUPUESTOS_USUARIOS_USUARIOID"))
                {
                    messages.Add("El usuario asociado al presupuesto no existe o es inválido.");
                }
                else if (normalized.Contains("FK_INGRESOS_CATEGORIAS_CATEGORIAID"))
                {
                    messages.Add("La categoría seleccionada para el ingreso no existe. Revisá las categorías.");
                }
                else if (normalized.Contains("FK_GASTOS_CATEGORIAS_CATEGORIAID"))
                {
                    messages.Add("La categoría seleccionada para el gasto no existe. Revisá las categorías.");
                }
                else
                {
                    messages.Add("Hay un problema con una relación (clave foránea). Revisá los datos relacionados (usuario, categoría, etc.).");
                }
            }
            // Conflictos de UNIQUE / índices
            else if (normalized.Contains("UNIQUE") ||
                     normalized.Contains("IX_") ||
                     normalized.Contains("DUPLICATE"))
            {
                messages.Add("Ya existe un registro con estos datos. No se permiten duplicados.");
            }
            else
            {
                messages.Add("Ocurrió un error al guardar los cambios en la base de datos.");
            }

            return messages;
        }
    }

    // Objeto de respuesta de error estándar para el front
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public List<string> Messages { get; set; } = new();
        public string? TraceId { get; set; }
    }
}
