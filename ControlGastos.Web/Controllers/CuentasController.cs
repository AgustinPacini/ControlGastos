using ControlGastos.Application.Cuenta_CQRS.Command;
using ControlGastos.Application.Cuenta_CQRS.Queries;
using ControlGastos.Application.Cuenta_CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ControlGastos.Web.Extensions;

namespace ControlGastos.Web.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    [Authorize]
    public class CuentasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CuentasController(IMediator mediator)
        {
            _mediator = mediator;
        }

       

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarioId = User.GetUsuarioId();
            var cuentas = await _mediator.Send(new GetCuentasByUsuarioQuery(usuarioId));
            return Ok(cuentas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = User.GetUsuarioId();
            var cuenta = await _mediator.Send(new GetCuentaByIdQuery(id, usuarioId));
            if (cuenta == null)
                return NotFound(new { message = "Cuenta no encontrada" });

            return Ok(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CuentaDto dto)
        {
            var usuarioId = User.GetUsuarioId();
            var id = await _mediator.Send(new CreateCuentaCommand(usuarioId, dto));
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CuentaDto dto)
        {
            var usuarioId = User.GetUsuarioId();
            var ok = await _mediator.Send(new UpdateCuentaCommand(id, usuarioId, dto));
            if (!ok)
                return NotFound(new { message = "Cuenta no encontrada" });

            return Ok(new { message = "Cuenta actualizada correctamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = User.GetUsuarioId();
            var ok = await _mediator.Send(new DeleteCuentaCommand(id, usuarioId));
            if (!ok)
                return NotFound(new { message = "Cuenta no encontrada" });

            return Ok(new { message = "Cuenta eliminada correctamente" });
        }
    }
}
