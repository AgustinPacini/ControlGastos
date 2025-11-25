using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cuenta_CQRS.Command
{
    public record UpdateCuentaCommand(int Id, int UsuarioId, CuentaDto Cuenta) : IRequest<bool>;
}
