using ControlGastos.Application.Ingreso_CQRS.Commands;
using ControlGastos.Application.Ingreso_CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace ControlGastos.Web.Controllers
{
    /// <summary>
    /// Endpoints para gestionar los ingresos del usuario.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IngresoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private int GetUsuarioId()
        {
            var claim = User.FindFirst("sub");
            if (claim == null)
                throw new Exception("No se encontró el id de usuario en el token.");

            return int.Parse(claim.Value);
        }
        public IngresoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crea un nuevo ingreso.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IngresosDto dto)
        {
            var usuarioId = GetUsuarioId();
            var id = await _mediator.Send(new CreateIngresoCommand(usuarioId, dto));
            return CreatedAtAction(nameof(GetAll), new { id }, new { id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarioId = GetUsuarioId();
            var result = await _mediator.Send(new GetAllIngresosQuery(usuarioId));
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = GetUsuarioId();
            var ingreso = await _mediator.Send(new GetIngresoByIdQuery(id, usuarioId));
            if (ingreso is null) return NotFound(new { message = "Ingreso no encontrado" });
            return Ok(ingreso);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] IngresosDto dto)
        {
            var usuarioId = GetUsuarioId();
            var result = await _mediator.Send(new UpdateIngresoCommand(id, usuarioId, dto));
            if (!result) return NotFound(new { message = "Ingreso no encontrado" });

            return Ok(new { message = "Ingreso actualizado con éxito" });
        }
    }
}
