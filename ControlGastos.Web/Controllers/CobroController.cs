using ControlGastos.Application.Cobro_CQRS.Commands;
using ControlGastos.Application.Cobro_CQRS.Queries;
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
        public async Task<IActionResult> Post([FromBody] CreateCobroCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { message = "Cobro creado", id });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cobros = await _mediator.Send(new GetAllCobrosQuery());
            return Ok(cobros);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCobroCommand(id));

            if (!result)
            {
                return NotFound(new { message = "No se encontró el Cobro para eliminar" });
            }

            return Ok(new { message = "Cobro eliminado con éxito" });
        }

    }
}
