
using ControlGastos.Application.Ingreso_CQRS.Commands;
using ControlGastos.Application.Ingreso_CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControlGastos.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngresoController : Controller
    {
        private readonly IMediator _mediator;

        public IngresoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateIngresoCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { message = "Ingreso creado", id });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cobros = await _mediator.Send(new GetAllIngresosQuery());
            return Ok(cobros);
        }
        [HttpDelete("{id}")]
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
