using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Exceptions;
using ControlGastos.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ControlGastos.Application.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IValidator<LoginDto> _validator;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IUsuarioRepository usuarioRepository,
            IJwtTokenService jwtTokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IValidator<LoginDto> validator,
            ILogger<LoginCommandHandler> logger)
        {
            _usuarioRepository = usuarioRepository;
            _jwtTokenService = jwtTokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginDto = request.loginDto;
            var nombreUsuario = request.loginDto.NombreUsuario?.Trim();

            // 1. Validar el DTO con FluentValidation
            var validationResult = await _validator.ValidateAsync(loginDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                throw new Exception($"Errores de validación: {string.Join(", ", errors)}");
            }
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(request.loginDto.Password))
            {
                throw new ValidationException("El usuario y la contraseña son obligatorios.");
            }

            // 2. Buscar el usuario por nombreUsuario
            var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(nombreUsuario);
            if (usuario == null)
            {
                _logger.LogWarning(
                    "Intento de login fallido - usuario no encontrado. NombreUsuario: {NombreUsuario}",
                    nombreUsuario);

                // Excepción de dominio → 401 en middleware
                throw new InvalidCredentialsException();
            }

            // 3. Verificar la contraseña usando BCrypt
            bool esValido = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);
            if (!esValido)
            {
                _logger.LogWarning(
          "Intento de login fallido - contraseña inválida. NombreUsuario: {NombreUsuario}",
          nombreUsuario);

                throw new InvalidCredentialsException();
            }

            // 4. Generar el Access Token (JWT)
            var accessToken = _jwtTokenService.GenerarToken(usuario.NombreUsuario, usuario.Id);

            // 5. Generar el Refresh Token
            var refreshTokenString = GenerarRefreshTokenString();
            var refreshToken = new RefreshToken
            {
                UsuarioId = usuario.Id,
                Token = refreshTokenString,
                Expira = DateTime.UtcNow.AddDays(7), // Expira en 7 días
                Revocado = false
            };

            // 6. Guardar el Refresh Token en la base de datos
            await _refreshTokenRepository.AddAsync(refreshToken);

            // 7. Retornar ambos tokens
            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenString
            };
        }

        private string GenerarRefreshTokenString()
        {
            // Genera un Refresh Token seguro, por ejemplo, combinando GUIDs
            return Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        }
    }
}
