using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace ControlGastos.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUsuarioId(this ClaimsPrincipal user)
        {
            var claim =
                user.FindFirst(ClaimTypes.NameIdentifier) ??
                user.FindFirst("usuarioId") ??
                user.FindFirst(JwtRegisteredClaimNames.Sub);

            if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
                throw new UnauthorizedAccessException("El token no contiene un identificador de usuario válido.");

            if (!int.TryParse(claim.Value, out var usuarioId))
                throw new UnauthorizedAccessException("El identificador de usuario del token es inválido.");

            return usuarioId;
        }
    }
}
