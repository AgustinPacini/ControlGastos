using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Categoria_CQRS.Commands
{
    public record DeleteCategoriaCommand(int Id) : IRequest<bool>;
}
