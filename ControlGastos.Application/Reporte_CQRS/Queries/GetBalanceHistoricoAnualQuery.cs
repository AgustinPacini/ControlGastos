using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Reporte_CQRS.Queries
{
    public record GetBalanceHistoricoAnualQuery(int UsuarioId, int Anio)
        : IRequest<List<BalanceHistoricoItem>>;

   
}
