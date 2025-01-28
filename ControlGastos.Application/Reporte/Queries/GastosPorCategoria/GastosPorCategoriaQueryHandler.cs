using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Reporte.Queries.GastosPorCategoria
{
    public class GastosPorCategoriaQueryHandler : IRequestHandler<GastosPorCategoriaQuery, List<GastoPorCategoriaResult>>
    {
        

        private readonly IBaseRepository<Gasto> _gastoRepository;

        public GastosPorCategoriaQueryHandler(IBaseRepository<Gasto> gastoRepository)
        {
            _gastoRepository = gastoRepository;
        }

        public async Task<List<GastoPorCategoriaResult>> Handle(GastosPorCategoriaQuery request, CancellationToken cancellationToken)
        {
            var gastos = await _gastoRepository.GetAllAsync();

            var resultado = gastos
                .Where(g => g.Fecha.Month == request.Mes && g.Fecha.Year == request.Anio)
                .GroupBy(g => g.Categoria.Nombre)
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
