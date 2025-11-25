using ControlGastos.Application.Gasto_CQRS.Commands;
using ControlGastos.Application.Gasto_CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControlGastos.Web.Controllers
{
    /// <summary>
    /// Endpoints para gestionar los gastos del usuario.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GastosController : ControllerBase
    {
        private int GetUsuarioId()
        {
            var claim = User.FindFirst("sub");
            if (claim == null)
                throw new Exception("No se encontró el id de usuario en el token.");

            return int.Parse(claim.Value);
        }
        private readonly IMediator _mediator;

        public GastosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crea un nuevo gasto.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GastoDto gastoDto)
        {
            if (gastoDto is null)
                return BadRequest(new { message = "Los datos del gasto son requeridos." });

            var usuarioId = GetUsuarioId();
            var gastoId = await _mediator.Send(new CreateGastoCommand(usuarioId, gastoDto));

            return CreatedAtAction(nameof(GetAll), new { id = gastoId }, new { id = gastoId });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarioId = GetUsuarioId();
            var gastos = await _mediator.Send(new GetAllGastosQuery(usuarioId));
            return Ok(gastos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = GetUsuarioId();
            var gasto = await _mediator.Send(new GetGastoByIdQuery(id, usuarioId));
            if (gasto is null) return NotFound(new { message = "Gasto no encontrado" });
            return Ok(gasto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GastoDto dto)
        {
            var usuarioId = GetUsuarioId();
            var result = await _mediator.Send(new UpdateGastoCommand(id, usuarioId, dto));
            if (!result) return NotFound(new { message = "Gasto no encontrado" });

            return Ok(new { message = "Gasto actualizado correctamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = GetUsuarioId();
            var result = await _mediator.Send(new DeleteGastoCommand(id, usuarioId));
            if (!result) return NotFound(new { message = "Gasto no encontrado" });

            return Ok(new { message = "Gasto eliminado correctamente" });
        }
    }
}
