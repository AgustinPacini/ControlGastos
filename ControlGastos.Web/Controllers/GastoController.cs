using ControlGastos.Application.Gasto_CQRS.Commands;
using ControlGastos.Application.Gasto_CQRS.Queries;

using ControlGastos.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ControlGastos.Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GastosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GastosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGasto([FromBody] CreateGastoDto gastoDto)
        {
            // Enviamos un CreateGastoCommand con el DTO
            var gastoId = await _mediator.Send(new CreateGastoCommand(gastoDto));

            return Ok(new { message = "Gasto creado", id = gastoId });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var gastos = await _mediator.Send(new GetAllGastosQuery());
            return Ok(gastos);
        }
    }
}
