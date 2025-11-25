using ControlGastos.Application.Gasto_CQRS.Commands;
using ControlGastos.Application.Gasto_CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Crea un nuevo gasto.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GastoDto gastoDto)
        {
            if (gastoDto is null)
                return BadRequest(new { message = "Los datos del gasto son requeridos." });

            var gastoId = await _mediator.Send(new CreateGastoCommand(gastoDto));

            return CreatedAtAction(nameof(GetAll), new { id = gastoId }, new { id = gastoId });
        }

        /// <summary>
        /// Obtiene el listado completo de gastos.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var gastos = await _mediator.Send(new GetAllGastosQuery());
            return Ok(gastos);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var gasto = await _mediator.Send(new GetGastoByIdQuery(id));
            if (gasto is null) return NotFound(new { message = "Gasto no encontrado" });
            return Ok(gasto);
        }

        /// <summary>
        /// Actualiza un gasto existente.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GastoDto dto)
        {
            var result = await _mediator.Send(new UpdateGastoCommand(id, dto));
            if (!result) return NotFound(new { message = "Gasto no encontrado" });

            return Ok(new { message = "Gasto actualizado correctamente" });
        }

        /// <summary>
        /// Elimina un gasto existente.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteGastoCommand(id));
            if (!result) return NotFound(new { message = "Gasto no encontrado" });

            return Ok(new { message = "Gasto eliminado correctamente" });
        }
    }
}
