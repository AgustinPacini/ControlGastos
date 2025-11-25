using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.GastoFijo_CQRS.Command
{
    public record CreateGastoFijoCommand(int UsuarioId, GastoFijoDto Gasto) : IRequest<int>;
}
