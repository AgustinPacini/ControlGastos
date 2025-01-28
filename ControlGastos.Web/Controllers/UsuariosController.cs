using ControlGastos.Application.Login;
using ControlGastos.Application.Usuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ControlGastos.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuariosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/usuarios/registro
        [HttpPost("registro")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegisterUsuarioDto dto)
        {
            var id = await _mediator.Send(new RegisterUsuarioCommand(dto));
            return Ok(new { message = "Usuario registrado con éxito", usuarioId = id });
        }

        // POST /api/usuarios/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _mediator.Send(new LoginCommand(loginDto));
            return Ok(response); // Retorna AccessToken y RefreshToken
        }

        // POST /api/usuarios/refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            var response = await _mediator.Send(new RefreshTokenCommand(dto));
            return Ok(response);
        }
    }
}

