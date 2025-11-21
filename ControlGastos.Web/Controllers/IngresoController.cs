using ControlGastos.Application.Ingreso_CQRS.Commands;
using ControlGastos.Application.Ingreso_CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var id = await _mediator.Send(new CreateIngresoCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        /// <summary>
        /// Obtiene el listado completo de ingresos.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ingresos = await _mediator.Send(new GetAllIngresosQuery());
            return Ok(ingresos);
        }

        /// <summary>
        /// Obtiene un ingreso por su Id.
        /// </summary>
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            // Por ahora no existe un Query específico por Id.
            // Se deja como placeholder para futura implementación.
            return NoContent();
        }

        /// <summary>
        /// Elimina un ingreso por su Id.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteIngresoCommand(id));

            if (!result)
            {
                return NotFound(new { message = "No se encontró el ingreso para eliminar" });
            }

            return Ok(new { message = "Ingreso eliminado con éxito" });
        }
    }
}
