using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Reporte.Queries.BalanceMensual
{
    
    public record BalanceMensualQuery(int Mes, int Anio) : IRequest<BalanceMensualResult>;

}
