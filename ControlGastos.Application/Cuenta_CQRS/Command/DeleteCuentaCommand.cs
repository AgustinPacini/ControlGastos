using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cuenta_CQRS.Command
{
    public record DeleteCuentaCommand(int Id, int UsuarioId) : IRequest<bool>;
}
