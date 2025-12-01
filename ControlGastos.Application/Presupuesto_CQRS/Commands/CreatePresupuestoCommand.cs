using ControlGastos.Application.Gasto_CQRS.Commands;
using ControlGastos.Application.Presupuesto_CQRS.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Presupuesto.Commands
{
    public record CreatePresupuestoCommand(int UsuarioId, PresupuestoDto PresupuestoDto) : IRequest<int>;
}
