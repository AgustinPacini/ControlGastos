using ControlGastos.Application.Export_CQRS.ControlGastos.Application.Export_CQRS;
using ControlGastos.Application.Export_CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ControlGastos.Application.Export_CQRS.Queries;
using ControlGastos.Web.Extensions;

namespace ControlGastos.Web.Controllers
{
    [ApiController]
    [Route("api/exportaciones")]
    [Authorize]
    public class ExportacionesController : Controller
    {
        
       
            private readonly IMediator _mediator;

            public ExportacionesController(IMediator mediator)
            {
                _mediator = mediator;
            }



            // GET api/exportaciones/gastos?desde=2025-01-01&hasta=2025-12-31
            [HttpGet("gastos")]
            public async Task<IActionResult> ExportGastos(
                [FromQuery] DateTime? desde,
                [FromQuery] DateTime? hasta)
            {
            var usuarioId = User.GetUsuarioId();
            var bytes = await _mediator.Send(new ExportGastosCsvQuery(usuarioId, desde, hasta));

                var fileName = $"gastos_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                return File(bytes, "text/csv", fileName);
            }

            // GET api/exportaciones/ingresos?desde=2025-01-01&hasta=2025-12-31
            [HttpGet("ingresos")]
            public async Task<IActionResult> ExportIngresos(
                [FromQuery] DateTime? desde,
                [FromQuery] DateTime? hasta)
            {
            var usuarioId = User.GetUsuarioId();
            var bytes = await _mediator.Send(new ExportIngresosCsvQuery(usuarioId, desde, hasta));

                var fileName = $"ingresos_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                return File(bytes, "text/csv", fileName);
            }
        
    }
}
