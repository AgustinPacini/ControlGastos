using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IValidator<LoginDto> _validator;

        public LoginCommandHandler(
            IUsuarioRepository usuarioRepository,
            IJwtTokenService jwtTokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IValidator<LoginDto> validator)
        {
            _usuarioRepository = usuarioRepository;
            _jwtTokenService = jwtTokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _validator = validator;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginDto = request.loginDto;

            // 1. Validar el DTO con FluentValidation
            var validationResult = await _validator.ValidateAsync(loginDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                throw new Exception($"Errores de validación: {string.Join(", ", errors)}");
            }

            // 2. Buscar el usuario por nombreUsuario
            var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(loginDto.NombreUsuario);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            // 3. Verificar la contraseña usando BCrypt
            bool esValido = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);
            if (!esValido)
            {
                throw new Exception("Contraseña incorrecta.");
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
