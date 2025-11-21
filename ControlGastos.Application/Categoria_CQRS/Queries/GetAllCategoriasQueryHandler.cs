using ControlGastos.Application.Categoria_CQRS.Commands;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Categoria_CQRS.Queries
{
    public class GetAllCategoriasQueryHandler
        : IRequestHandler<GetAllCategoriasQuery, List<CategoriaDto>>
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public GetAllCategoriasQueryHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<List<CategoriaDto>> Handle(
            GetAllCategoriasQuery request,
            CancellationToken cancellationToken)
        {
            var categorias = await _categoriaRepository.GetAllAsync();

            return categorias
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    TipoCategoria = c.TipoCategoria
                })
                .ToList();
        }
    }
}
