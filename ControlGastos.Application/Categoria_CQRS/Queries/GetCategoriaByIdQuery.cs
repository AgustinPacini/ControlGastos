using ControlGastos.Application.Categoria_CQRS.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Categoria_CQRS.Queries
{
    public record GetCategoriaByIdQuery(int Id) : IRequest<CategoriaDto?>;
}
