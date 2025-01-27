using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cobro_CQRS.Commands
{
    public record CreateCobroCommand(string Descripcion, decimal Monto, DateTime Fecha, int TipoGasto) : IRequest<int>;
    
}
