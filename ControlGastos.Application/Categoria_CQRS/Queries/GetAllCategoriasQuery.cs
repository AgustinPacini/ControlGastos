using ControlGastos.Application.Categoria_CQRS.Commands;
using MediatR;
using System.Collections.Generic;

namespace ControlGastos.Application.Categoria_CQRS
{
    public record GetAllCategoriasQuery() : IRequest<List<CategoriaDto>>;
}
