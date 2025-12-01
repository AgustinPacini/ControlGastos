using ControlGastos.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Infrastructure.Services
{


    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerarToken(string nombreUsuario, int usuarioId)
        {
            // 1. Tomar configuraciones de appsettings
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = _config["Jwt:Key"];

            // 2. Crear la firma
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 3. Crear los claims
            var claims = new List<Claim>
        {
            // 👇 este es el claim que probablemente está esperando tu código
            new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),

            // seguimos manteniendo 'sub' por compatibilidad
            new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),

            // nombre de usuario
            new Claim(ClaimTypes.Name, nombreUsuario ?? string.Empty),
            new Claim("nombreUsuario", nombreUsuario ?? string.Empty)
            // acá podés sumar roles etc si querés
        };

            // 4. Configurar el token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            // 5. Escribir el token en string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
