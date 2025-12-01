using ControlGastos.Application.MetasAhorro_CQRS.Command;
using ControlGastos.Application.MetasAhorro_CQRS.Queries;
using ControlGastos.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlGastos.Web.Controllers
{
    [ApiController]
    [Route("api/metas-ahorro")]
    [Authorize]
    public class MetasAhorroController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MetasAhorroController(IMediator mediator)
        {
            _mediator = mediator;
        }

      

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarioId = User.GetUsuarioId();
            var metas = await _mediator.Send(new GetMetasAhorroByUsuarioQuery(usuarioId));
            return Ok(metas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDetalle(int id)
        {
            var usuarioId = User.GetUsuarioId();
            var detalle = await _mediator.Send(new GetMetaAhorroDetalleQuery(id, usuarioId));
            if (detalle == null)
                return NotFound(new { message = "Meta no encontrada" });

            return Ok(detalle);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MetaAhorroDto dto)
        {
            var usuarioId = User.GetUsuarioId();
            var id = await _mediator.Send(new CreateMetaAhorroCommand(usuarioId, dto));
            return CreatedAtAction(nameof(GetDetalle), new { id }, new { id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] MetaAhorroDto dto)
        {
            var usuarioId = User.GetUsuarioId();
            var ok = await _mediator.Send(new UpdateMetaAhorroCommand(id, usuarioId, dto));
            if (!ok) return NotFound(new { message = "Meta no encontrada" });

            return Ok(new { message = "Meta actualizada correctamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = User.GetUsuarioId();
            var ok = await _mediator.Send(new DeleteMetaAhorroCommand(id, usuarioId));
            if (!ok) return NotFound(new { message = "Meta no encontrada" });

            return Ok(new { message = "Meta eliminada correctamente" });
        }
    }
}
