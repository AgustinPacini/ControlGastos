using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cobro_CQRS.Commands
{
    public record DeleteCobroCommand(int Id) : IRequest<bool>;
}
