using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cuenta_CQRS.Command
{
    public record CreateCuentaCommand(int UsuarioId, CuentaDto Cuenta) : IRequest<int>;
}
