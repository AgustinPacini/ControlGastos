using ControlGastos.Application.GastoFijo_CQRS.Command;
using ControlGastos.Application.GastoFijo_CQRS.Queries;
using ControlGastos.Application.GastoFijo_CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlGastos.Web.Controllers
{
    [ApiController]
    [Route("api/gastos-fijos")]
    [Authorize]
    public class GastosFijosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GastosFijosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private int GetUsuarioId()
        {
            var claim = User.FindFirst("sub");
            if (claim == null)
                throw new Exception("No se encontró el id de usuario en el token.");
            return int.Parse(claim.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarioId = GetUsuarioId();
            var data = await _mediator.Send(new GetGastosFijosByUsuarioQuery(usuarioId));
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = GetUsuarioId();
            var entity = await _mediator.Send(new GetGastoFijoByIdQuery(id, usuarioId));
            if (entity == null)
                return NotFound(new { message = "Gasto fijo no encontrado" });

            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GastoFijoDto dto)
        {
            var usuarioId = GetUsuarioId();
            var id = await _mediator.Send(new CreateGastoFijoCommand(usuarioId, dto));
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GastoFijoDto dto)
        {
            var usuarioId = GetUsuarioId();
            var ok = await _mediator.Send(new UpdateGastoFijoCommand(id, usuarioId, dto));
            if (!ok)
                return NotFound(new { message = "Gasto fijo no encontrado" });

            return Ok(new { message = "Gasto fijo actualizado correctamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = GetUsuarioId();
            var ok = await _mediator.Send(new DeleteGastoFijoCommand(id, usuarioId));
            if (!ok)
                return NotFound(new { message = "Gasto fijo no encontrado" });

            return Ok(new { message = "Gasto fijo eliminado correctamente" });
        }
    }
}
