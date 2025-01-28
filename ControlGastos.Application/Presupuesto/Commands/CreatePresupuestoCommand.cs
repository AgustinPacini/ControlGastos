using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Presupuesto.Commands
{
    public record CreatePresupuestoCommand(string Nombre, decimal MontoLimite, int Mes, int Anio, int? CategoriaId) : IRequest<int>;
}
