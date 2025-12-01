using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Presupuesto_CQRS.Queries
{
    public record GetPresupuestosQuery(int UsuarioId) : IRequest<List<PresupuestoDto>>;
}
