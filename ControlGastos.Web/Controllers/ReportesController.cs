using ControlGastos.Application.Reporte.Queries.BalanceMensual;
using ControlGastos.Application.Reporte.Queries.GastosPorCategoria;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlGastos.Web.Controllers
{
    /// <summary>
    /// Endpoints de reportes agregados (balance mensual, gastos por categoría, etc.).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Devuelve el balance mensual (ingresos, gastos y diferencia) para el mes/año indicados.
        /// </summary>
        [HttpGet("balance-mensual")]
        public async Task<IActionResult> GetBalanceMensual([FromQuery] int mes, [FromQuery] int anio)
        {
            var query = new BalanceMensualQuery(mes, anio);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Devuelve los gastos agrupados por categoría para el mes/año indicados.
        /// </summary>
        [HttpGet("gastos-por-categoria")]
        public async Task<IActionResult> GetGastosPorCategoria([FromQuery] int mes, [FromQuery] int anio)
        {
            var query = new GastosPorCategoriaQuery(mes, anio);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
