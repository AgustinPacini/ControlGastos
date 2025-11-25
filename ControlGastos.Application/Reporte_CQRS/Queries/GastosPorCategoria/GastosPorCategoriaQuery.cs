using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Reporte.Queries.GastosPorCategoria
{
    public record GastosPorCategoriaQuery(int Mes, int Anio) : IRequest<List<GastoPorCategoriaResult>>;
}
