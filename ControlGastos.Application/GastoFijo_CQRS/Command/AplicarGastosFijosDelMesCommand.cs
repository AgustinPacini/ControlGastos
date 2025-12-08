using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.GastoFijo_CQRS.Command
{
    /// <summary>
    /// Aplica los gastos fijos del mes de la fecha indicada
    /// para un usuario específico. Devuelve cuántos gastos reales se generaron.
    /// </summary>
    public record AplicarGastosFijosDelMesCommand(
        int UsuarioId,
        DateTime FechaReferencia
    ) : IRequest<int>;
}
