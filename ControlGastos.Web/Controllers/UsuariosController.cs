using ControlGastos.Application.Login;
using ControlGastos.Application.Usuario;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlGastos.Web.Controllers
{
    /// <summary>
    /// Endpoints relacionados con usuarios: registro, login y refresh de tokens.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuariosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUsuarioDto dto)
        {
            var usuarioId = await _mediator.Send(new RegisterUsuarioCommand(dto));
            return Ok(new { id = usuarioId });
        }

        /// <summary>
        /// Realiza el login de un usuario y devuelve un AccessToken + RefreshToken.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _mediator.Send(new LoginCommand(loginDto));
            return Ok(response); // Retorna AccessToken y RefreshToken
        }

        /// <summary>
        /// Permite obtener un nuevo AccessToken a partir de un RefreshToken válido.
        /// </summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            var response = await _mediator.Send(new RefreshTokenCommand(dto));
            return Ok(response);
        }
    }
}
