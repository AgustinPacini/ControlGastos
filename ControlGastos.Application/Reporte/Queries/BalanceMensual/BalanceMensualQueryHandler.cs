using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Reporte.Queries.BalanceMensual
{
    public class BalanceMensualQueryHandler : IRequestHandler<BalanceMensualQuery, BalanceMensualResult>
    {
        private readonly IBaseRepository<Gasto> _gastosRepository;
        private readonly IBaseRepository<Ingresos> _ingresoRepository;

        public BalanceMensualQueryHandler(IBaseRepository<Gasto> gastosRepository, IBaseRepository<Ingresos> ingresoRepository)
        {
            _gastosRepository = gastosRepository;
            _ingresoRepository = ingresoRepository;
        }

        public async Task<BalanceMensualResult> Handle(BalanceMensualQuery request, CancellationToken cancellationToken)
        {
            var gastos = await _gastosRepository.GetAllAsync();
            var ingresos = await _ingresoRepository.GetAllAsync();

            var totalGastos = gastos
                .Where(g => g.Fecha.Month == request.Mes && g.Fecha.Year == request.Anio)
                .Sum(g => g.Monto);

            var totalIngresos = ingresos
                .Where(i => i.Fecha.Month == request.Mes && i.Fecha.Year == request.Anio)
                .Sum(i => i.Monto);

            return new BalanceMensualResult
            {
                TotalIngresos = totalIngresos,
                TotalGastos = totalGastos,
                Balance = totalIngresos - totalGastos
            };
        }
    }
}
