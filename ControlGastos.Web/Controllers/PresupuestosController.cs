using ControlGastos.Application.Presupuesto.Commands;
using ControlGastos.Application.Presupuesto_CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlGastos.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PresupuestosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PresupuestosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePresupuestoCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMensuales), new { id }, new { id });
        }

        /// <summary>
        /// Devuelve todos los presupuestos del mes con su uso vs límite.
        /// </summary>
        [HttpGet("mensuales")]
        public async Task<IActionResult> GetMensuales([FromQuery] int anio, [FromQuery] int mes)
        {
            var result = await _mediator.Send(new GetPresupuestosMensualesQuery(anio, mes));
            return Ok(result);
        }
    }
}
