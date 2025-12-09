using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Exceptions;
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
            

                var storedToken = await _refreshTokenRepo.GetByTokenAsync(request.Dto.RefreshToken);
                 var usuario = await _usuarioRepo.GetByIdAsync(storedToken.UsuarioId);
            if (storedToken == null)
                    throw new InvalidCredentialsException("Refresh token inválido.");

                if (storedToken.Revocado || storedToken.Expira <= DateTime.UtcNow)
                {
                    // Marcamos revocado si aún no lo estaba (ej: expirado)
                    if (!storedToken.Revocado)
                    {
                        storedToken.Revocado = true;
                        storedToken.RevocadoEn = DateTime.UtcNow;
                        storedToken.RevocadoPorIp = usuario.NombreUsuario;
                        await _refreshTokenRepo.UpdateAsync(storedToken);
                    }

                    throw new InvalidCredentialsException("Refresh token expirado o revocado.");
                }

                // Buscamos usuario
                
                if (usuario == null)
                    throw new KeyNotFoundException("No se encontró el usuario asociado al token.");

                // Revocamos el token anterior
                storedToken.Revocado = true;
                storedToken.RevocadoEn = DateTime.UtcNow;
                storedToken.RevocadoPorIp = usuario.NombreUsuario;
                await _refreshTokenRepo.UpdateAsync(storedToken);

                // Generamos nuevos tokens
                var newAccessToken = _jwtTokenService.GenerarToken(usuario.NombreUsuario,usuario.Id);
                var newRefreshTokenString = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

                var newRefreshToken = new RefreshToken
                {
                    UsuarioId = usuario.Id,
                    Token = newRefreshTokenString,
                    Creado = DateTime.UtcNow,
                    CreadoPorIp = usuario.NombreUsuario,
                    Expira = DateTime.UtcNow.AddDays(7),
                    Revocado = false
                };

                await _refreshTokenRepo.AddAsync(newRefreshToken);

                return new RefreshTokenResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshTokenString
                };
            }
          
    }
}
