using ControlGastos.Application.Cuenta_CQRS.Command;
using ControlGastos.Application.Cuenta_CQRS.Queries;
using ControlGastos.Application.Cuenta_CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var cuentas = await _mediator.Send(new GetCuentasByUsuarioQuery(usuarioId));
            return Ok(cuentas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = GetUsuarioId();
            var cuenta = await _mediator.Send(new GetCuentaByIdQuery(id, usuarioId));
            if (cuenta == null)
                return NotFound(new { message = "Cuenta no encontrada" });

            return Ok(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CuentaDto dto)
        {
            var usuarioId = GetUsuarioId();
            var id = await _mediator.Send(new CreateCuentaCommand(usuarioId, dto));
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CuentaDto dto)
        {
            var usuarioId = GetUsuarioId();
            var ok = await _mediator.Send(new UpdateCuentaCommand(id, usuarioId, dto));
            if (!ok)
                return NotFound(new { message = "Cuenta no encontrada" });

            return Ok(new { message = "Cuenta actualizada correctamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = GetUsuarioId();
            var ok = await _mediator.Send(new DeleteCuentaCommand(id, usuarioId));
            if (!ok)
                return NotFound(new { message = "Cuenta no encontrada" });

            return Ok(new { message = "Cuenta eliminada correctamente" });
        }
    }
}
