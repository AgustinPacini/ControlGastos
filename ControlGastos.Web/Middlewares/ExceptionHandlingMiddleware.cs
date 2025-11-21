using FluentValidation;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

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
                // Continúa con el siguiente middleware o la acción del controlador
                await _next(context);
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción no manejada que ocurra en el pipeline
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Por defecto, asumimos un error interno del servidor
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var errors = new List<string> { exception.Message };

            // 1) Errores de validación (FluentValidation)
            if (exception is ValidationException validationEx)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errors = validationEx.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
            }
            // 2) Errores de base de datos (DbUpdateException)
            else if (exception is DbUpdateException dbEx)
            {
                statusCode = (int)HttpStatusCode.InternalServerError;

                // Mensaje genérico
                errors = new List<string> { dbEx.Message };

                // Mensaje de la inner exception (el interesante)
                if (dbEx.InnerException != null)
                {
                    errors.Add(dbEx.InnerException.Message);

                    if (dbEx.InnerException.InnerException != null)
                    {
                        errors.Add(dbEx.InnerException.InnerException.Message);
                    }
                }
            }
            // Podrías verificar otros tipos de excepción (NotFound, Unauthorized, etc.)
            // else if (exception is NotFoundException notFoundEx) { ... }
            // else if (exception is MyCustomException customEx) { ... }

            // Log de la excepción (útil para debugging y monitoreo)
            _logger.LogError(exception, "Ocurrió un error: {Error}", exception.Message);

            // Construimos la respuesta de error
            var errorResponse = new ErrorResponse
            {
                StatusCode = statusCode,
                Messages = errors
            };

            // Configuramos la respuesta HTTP
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var json = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(json);
        }
    }

    // Este objeto representará la respuesta de error en formato JSON
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public List<string> Messages { get; set; } = new();
    }
}
