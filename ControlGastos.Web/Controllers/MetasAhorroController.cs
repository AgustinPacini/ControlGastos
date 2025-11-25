using ControlGastos.Application.MetasAhorro_CQRS.Command;
using ControlGastos.Application.MetasAhorro_CQRS.Queries;
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
            var metas = await _mediator.Send(new GetMetasAhorroByUsuarioQuery(usuarioId));
            return Ok(metas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDetalle(int id)
        {
            var usuarioId = GetUsuarioId();
            var detalle = await _mediator.Send(new GetMetaAhorroDetalleQuery(id, usuarioId));
            if (detalle == null)
                return NotFound(new { message = "Meta no encontrada" });

            return Ok(detalle);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MetaAhorroDto dto)
        {
            var usuarioId = GetUsuarioId();
            var id = await _mediator.Send(new CreateMetaAhorroCommand(usuarioId, dto));
            return CreatedAtAction(nameof(GetDetalle), new { id }, new { id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] MetaAhorroDto dto)
        {
            var usuarioId = GetUsuarioId();
            var ok = await _mediator.Send(new UpdateMetaAhorroCommand(id, usuarioId, dto));
            if (!ok) return NotFound(new { message = "Meta no encontrada" });

            return Ok(new { message = "Meta actualizada correctamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = GetUsuarioId();
            var ok = await _mediator.Send(new DeleteMetaAhorroCommand(id, usuarioId));
            if (!ok) return NotFound(new { message = "Meta no encontrada" });

            return Ok(new { message = "Meta eliminada correctamente" });
        }
    }
}
