using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Categoria_CQRS.Commands
{
    
    public record CreateCategoriaCommand(string Nombre, string? Descripcion) : IRequest<int>;

}
