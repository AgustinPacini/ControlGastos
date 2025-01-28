using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Commands
{
    public record CreateGastoCommand(CreateGastoDto GastoDto) : IRequest<int>;
    
}
