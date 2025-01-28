using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Login
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDto>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IJwtTokenService _jwtTokenService; // Lo usas para generar un nuevo JWT
        private readonly IUsuarioRepository _usuarioRepo;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepo,
            IJwtTokenService jwtTokenService,
            IUsuarioRepository usuarioRepo)
        {
            _refreshTokenRepo = refreshTokenRepo;
            _jwtTokenService = jwtTokenService;
            _usuarioRepo = usuarioRepo;
        }

        public async Task<RefreshTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenString = request.Dto.RefreshToken;

            // 1. Buscar el refresh token en BD
            var refreshToken = await _refreshTokenRepo.GetByTokenAsync(refreshTokenString);
            if (refreshToken == null)
                throw new Exception("Refresh token inválido.");

            // 2. Verificar si está vencido o revocado
            if (refreshToken.Expira < DateTime.Now)
                throw new Exception("Refresh token expirado.");

            if (refreshToken.Revocado)
                throw new Exception("Refresh token revocado.");

            // 3. Obtener el usuario asociado
            var usuario = await _usuarioRepo.GetByIdAsync(refreshToken.UsuarioId);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            // 4. Generar un nuevo Access Token
            var nuevoAccessToken = _jwtTokenService.GenerarToken(usuario.NombreUsuario, usuario.Id);

            // 5. (Opcional) Generar un nuevo Refresh Token para prolongar la sesión
            //     o puedes seguir usando el mismo refresh token hasta su fecha de expiración
            //     Aquí mostraremos cómo reemplazarlo:
            var nuevoRefreshTokenString = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
            refreshToken.Token = nuevoRefreshTokenString;
            refreshToken.Expira = DateTime.Now.AddDays(7); // Reinicia su vigencia
            // Marcamos como revocado "el viejo token"? 
            // En este caso, estamos reutilizando el MISMO registro, cambiando su token y fecha.
            // O podríamos revocar y crear uno nuevo. Depende de tu estrategia.

            await _refreshTokenRepo.UpdateAsync(refreshToken);

            return new RefreshTokenResponseDto
            {
                AccessToken = nuevoAccessToken,
                RefreshToken = nuevoRefreshTokenString
            };
        }
    }
}
