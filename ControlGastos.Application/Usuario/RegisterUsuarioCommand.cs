using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Usuario
{
    public record RegisterUsuarioCommand(RegisterUsuarioDto UsuarioDto) : IRequest<int>;
}
