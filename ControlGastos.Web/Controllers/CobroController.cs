using ControlGastos.Application.Gasto_CQRS.Commands;
using ControlGastos.Application.Gasto_CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControlGastos.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CobroController : Controller
    {
        private readonly IMediator _mediator;

        public CobroController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateGastoCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { message = "Cobro creado", id });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var gastos = await _mediator.Send(new GetAllGastosQuery());
            return Ok(gastos);
        }

    }
}
