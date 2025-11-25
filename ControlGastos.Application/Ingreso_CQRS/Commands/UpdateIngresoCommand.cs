using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Ingreso_CQRS.Commands
{
    public record UpdateIngresoCommand(int Id, IngresosDto IngresoDto) : IRequest<bool>;
}
