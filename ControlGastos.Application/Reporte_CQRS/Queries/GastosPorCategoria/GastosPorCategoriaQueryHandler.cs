using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ControlGastos.Application.Reporte.Queries.GastosPorCategoria
{
    /// <summary>
    /// Handler que devuelve el total gastado agrupado por categoría para un mes/año determinados.
    /// </summary>
    public class GastosPorCategoriaQueryHandler : IRequestHandler<GastosPorCategoriaQuery, List<GastoPorCategoriaResult>>
    {
        private readonly IGastoRepository _gastoRepository;

        public GastosPorCategoriaQueryHandler(IGastoRepository gastoRepository)
        {
            _gastoRepository = gastoRepository;
        }

        public async Task<List<GastoPorCategoriaResult>> Handle(GastosPorCategoriaQuery request, CancellationToken cancellationToken)
        {
            // Traemos solo los gastos del mes/año solicitado.
            var gastos = await _gastoRepository.GetByMonth(request.Anio, request.Mes);

            var resultado = gastos
                .GroupBy(g => g.Categoria?.Nombre ?? "Sin categoría")
                .Select(g => new GastoPorCategoriaResult
                {
                    Categoria = g.Key,
                    TotalGastado = g.Sum(x => x.Monto)
                })
                .ToList();

            return resultado;
        }
    }
}
