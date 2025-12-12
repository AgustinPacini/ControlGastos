using ControlGastos.Application.Gasto_CQRS.Commands;
using ControlGastos.Application.Gasto_CQRS.Queries;
using ControlGastos.Web.Extensions;
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
        private readonly IMediator _mediator;

        public GastosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GastoDto gastoDto, CancellationToken cancellationToken)
        {
            if (gastoDto is null)
                return BadRequest(new { message = "Los datos del gasto son requeridos." });

            var usuarioId = User.GetUsuarioId();
            var gastoId = await _mediator.Send(new CreateGastoCommand(usuarioId, gastoDto), cancellationToken);

            return CreatedAtAction(nameof(GetAll), new { id = gastoId }, new { id = gastoId });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usuarioId = User.GetUsuarioId();
            var gastos = await _mediator.Send(new GetAllGastosQuery(usuarioId), cancellationToken);
            return Ok(gastos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var usuarioId = User.GetUsuarioId();
            var gasto = await _mediator.Send(new GetGastoByIdQuery(id, usuarioId), cancellationToken);
            if (gasto is null) return NotFound(new { message = "Gasto no encontrado" });
            return Ok(gasto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GastoDto dto, CancellationToken cancellationToken)
        {
            var usuarioId = User.GetUsuarioId();
            var result = await _mediator.Send(new UpdateGastoCommand(id, usuarioId, dto), cancellationToken);
            if (!result) return NotFound(new { message = "Gasto no encontrado" });

            return Ok(new { message = "Gasto actualizado correctamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var usuarioId = User.GetUsuarioId();
            var result = await _mediator.Send(new DeleteGastoCommand(id, usuarioId), cancellationToken);
            if (!result) return NotFound(new { message = "Gasto no encontrado" });

            return Ok(new { message = "Gasto eliminado correctamente" });
        }
    }
}
