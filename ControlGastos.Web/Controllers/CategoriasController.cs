
using ControlGastos.Application.Categoria_CQRS.Commands;
using ControlGastos.Application.Categoria_CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ControlGastos.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // o [AllowAnonymous] si querés que cualquiera las vea
    public class CategoriasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _mediator.Send(new GetAllCategoriasQuery());
            return Ok(categorias);
        }

        public class CreateCategoriaRequest
        {
            public string Nombre { get; set; } = string.Empty;
            public string? Descripcion { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoriaRequest request)
        {
            var id = await _mediator.Send(
                new CreateCategoriaCommand(request.Nombre, request.Descripcion)
            );

            return CreatedAtAction(nameof(GetAll), new { id }, new { id });
        }
    }
}
