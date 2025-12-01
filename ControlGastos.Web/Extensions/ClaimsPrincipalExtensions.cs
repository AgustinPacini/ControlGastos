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
                throw new Exception("No se encontró el id de usuario en el token.");

            return int.Parse(claim.Value);
        }
    }
}
