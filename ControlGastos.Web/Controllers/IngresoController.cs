using ControlGastos.Application.Ingreso_CQRS.Commands;
using ControlGastos.Application.Ingreso_CQRS.Queries;
using ControlGastos.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace ControlGastos.Web.Controllers
{
    /// <summary>
    /// Endpoints para gestionar los ingresos del usuario.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IngresoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IngresoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IngresosDto ingresosDto, CancellationToken cancellationToken)
        {
            if (ingresosDto is null)
                return BadRequest(new { message = "Los datos del ingreso son requeridos." });

            var usuarioId = User.GetUsuarioId();
            var ingresoId = await _mediator.Send(new CreateIngresoCommand(usuarioId, ingresosDto), cancellationToken);

            return CreatedAtAction(nameof(GetAll), new { id = ingresoId }, new { id = ingresoId });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usuarioId = User.GetUsuarioId();
            var ingresos = await _mediator.Send(new GetAllIngresosQuery(usuarioId), cancellationToken);
            return Ok(ingresos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var usuarioId = User.GetUsuarioId();
            var ingreso = await _mediator.Send(new GetIngresoByIdQuery(id, usuarioId), cancellationToken);
            if (ingreso is null) return NotFound(new { message = "Ingreso no encontrado" });
            return Ok(ingreso);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] IngresosDto dto, CancellationToken cancellationToken)
        {
            var usuarioId = User.GetUsuarioId();
            var result = await _mediator.Send(new UpdateIngresoCommand(id, usuarioId, dto), cancellationToken);
            if (!result) return NotFound(new { message = "Ingreso no encontrado" });

            return Ok(new { message = "Ingreso actualizado con éxito" });
        }
    }
}
