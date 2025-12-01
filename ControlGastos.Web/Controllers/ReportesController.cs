using ControlGastos.Application.Reporte.Queries.BalanceMensual;
using ControlGastos.Application.Reporte.Queries.GastosPorCategoria;
using ControlGastos.Application.Reporte_CQRS;
using ControlGastos.Application.Reporte_CQRS.Queries;
using ControlGastos.Web.Extensions;
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
            var usuarioId = User.GetUsuarioId();
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
            var usuarioId = User.GetUsuarioId();
            var result = await _mediator.Send(new GetTopCategoriasQuery(usuarioId, desde, hasta, topN));
            return Ok(result);
        }

        // GET api/reportes/tendencias-mensuales?meses=6
        [HttpGet("tendencias-mensuales")]
        public async Task<IActionResult> GetTendenciasMensuales([FromQuery] int meses = 6)
        {
            var usuarioId = User.GetUsuarioId();
            var result = await _mediator.Send(new GetTendenciasMensualesQuery(usuarioId, meses));
            return Ok(result);
        }
        /// <summary>
        /// Resumen general para el dashboard:
        /// - Balance mensual (ingresos, gastos, balance)
        /// - Top categorías del mes
        /// - Tendencias de los últimos N meses
        /// </summary>
        /// GET api/reportes/resumen-dashboard?mes=11&anio=2025&mesesTendencia=6&topN=5
        [HttpGet("resumen-dashboard")]
        public async Task<IActionResult> GetResumenDashboard(
            [FromQuery] int mes,
            [FromQuery] int anio,
            [FromQuery] int mesesTendencia = 6,
            [FromQuery] int topN = 5)
        {
            var usuarioId = User.GetUsuarioId();

            // 1) Balance mensual (ahora mismo es global; si querés después lo adaptamos a usuario)
            var balance = await _mediator.Send(new BalanceMensualQuery(mes, anio));

            // 2) Top categorías del mes para este usuario
            var desde = new DateTime(anio, mes, 1);
            var hasta = desde.AddMonths(1).AddDays(-1);

            var topCategorias = await _mediator.Send(
                new GetTopCategoriasQuery(usuarioId, desde, hasta, topN));

            // 3) Tendencias últimas X meses para este usuario
            var tendencias = await _mediator.Send(
                new GetTendenciasMensualesQuery(usuarioId, mesesTendencia));

            var dto = new DashboardResumenDto
            {
                TotalIngresosMes = balance.TotalIngresos,
                TotalGastosMes = balance.TotalGastos,
                BalanceMes = balance.Balance,
                TopCategorias = topCategorias,
                Tendencias = tendencias
            };

            return Ok(dto);
        }
    }
}
