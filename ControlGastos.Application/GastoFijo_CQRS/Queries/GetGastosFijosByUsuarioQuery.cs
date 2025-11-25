using ControlGastos.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.GastoFijo_CQRS.Queries
{
    public record GetGastosFijosByUsuarioQuery(int UsuarioId) : IRequest<List<GastoFijo>>;
}
