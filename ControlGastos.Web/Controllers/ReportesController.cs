using ControlGastos.Application.Reporte.Queries.BalanceMensual;
using ControlGastos.Application.Reporte.Queries.GastosPorCategoria;
using ControlGastos.Application.Reporte_CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        private int GetUsuarioId()
        {
            var claim = User.FindFirst("sub");
            if (claim == null)
                throw new Exception("No se encontró el id de usuario en el token.");

            return int.Parse(claim.Value);
        }
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
        [HttpGet("balance-historico")]
        public async Task<IActionResult> GetBalanceHistorico([FromQuery] int anio)
        {
            var usuarioId = GetUsuarioId();
            var result = await _mediator.Send(new GetBalanceHistoricoAnualQuery(usuarioId, anio));
            return Ok(result);
        }

        // GET api/reportes/top-categorias?desde=2025-01-01&hasta=2025-12-31&topN=5
        [HttpGet("top-categorias")]
        public async Task<IActionResult> GetTopCategorias(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta,
            [FromQuery] int topN = 5)
        {
            var usuarioId = GetUsuarioId();
            var result = await _mediator.Send(new GetTopCategoriasQuery(usuarioId, desde, hasta, topN));
            return Ok(result);
        }

        // GET api/reportes/tendencias-mensuales?meses=6
        [HttpGet("tendencias-mensuales")]
        public async Task<IActionResult> GetTendenciasMensuales([FromQuery] int meses = 6)
        {
            var usuarioId = GetUsuarioId();
            var result = await _mediator.Send(new GetTendenciasMensualesQuery(usuarioId, meses));
            return Ok(result);
        }
    }
}
