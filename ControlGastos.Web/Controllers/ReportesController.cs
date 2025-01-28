using ControlGastos.Application.Reporte.Queries.BalanceMensual;
using ControlGastos.Application.Reporte.Queries.GastosPorCategoria;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ControlGastos.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("balance-mensual")]
        public async Task<IActionResult> GetBalanceMensual([FromQuery] int mes, [FromQuery] int anio)
        {
            var query = new BalanceMensualQuery(mes, anio);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("gastos-por-categoria")]
        public async Task<IActionResult> GetGastosPorCategoria([FromQuery] int mes, [FromQuery] int anio)
        {
            var query = new GastosPorCategoriaQuery(mes, anio);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
