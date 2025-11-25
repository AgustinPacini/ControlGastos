
using ControlGastos.Application.Categoria_CQRS.Commands;
using ControlGastos.Application.Categoria_CQRS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ControlGastos.Application.Categoria_CQRS.Queries;

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
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var categoria = await _mediator.Send(new GetCategoriaByIdQuery(id));
            if (categoria is null) return NotFound(new { message = "Categoría no encontrada" });
            return Ok(categoria);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoriaDto request)
        {
            var result = await _mediator.Send(
                new UpdateCategoriaCommand(id, request.Nombre, request.Descripcion, request.TipoCategoria)
            );

            if (!result) return NotFound(new { message = "Categoría no encontrada" });

            return Ok(new { message = "Categoría actualizada correctamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCategoriaCommand(id));

            if (!result) return NotFound(new { message = "Categoría no encontrada" });

            return Ok(new { message = "Categoría eliminada correctamente" });
        }
    }
}
