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

    public class GetCategoriaByIdQueryHandler : IRequestHandler<GetCategoriaByIdQuery, CategoriaDto?>
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public GetCategoriaByIdQueryHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<CategoriaDto?> Handle(GetCategoriaByIdQuery request, CancellationToken cancellationToken)
        {
            var c = await _categoriaRepository.GetByIdAsync(request.Id);
            if (c is null) return null;

            return new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                TipoCategoria = c.TipoCategoria
            };
        }
    }
}
