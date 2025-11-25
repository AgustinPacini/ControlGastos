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
    public class GetBalanceHistoricoAnualQueryHandler
        : IRequestHandler<GetBalanceHistoricoAnualQuery, List<BalanceHistoricoItem>>
    {
        private readonly IBaseRepository<Gasto> _gastoRepo;
        private readonly IBaseRepository<Ingresos> _ingresoRepo;

        public GetBalanceHistoricoAnualQueryHandler(
            IBaseRepository<Gasto> gastoRepo,
            IBaseRepository<Ingresos> ingresoRepo)
        {
            _gastoRepo = gastoRepo;
            _ingresoRepo = ingresoRepo;
        }

        public async Task<List<BalanceHistoricoItem>> Handle(GetBalanceHistoricoAnualQuery request, CancellationToken cancellationToken)
        {
            var gastos = (await _gastoRepo.GetAllAsync())
                .Where(g => g.UsuarioId == request.UsuarioId && g.Fecha.Year == request.Anio)
                .ToList();

            var ingresos = (await _ingresoRepo.GetAllAsync())
                .Where(i => i.UsuarioId == request.UsuarioId && i.Fecha.Year == request.Anio)
                .ToList();

            var resultado = new List<BalanceHistoricoItem>();

            for (int mes = 1; mes <= 12; mes++)
            {
                var totalGastos = gastos.Where(g => g.Fecha.Month == mes).Sum(g => g.Monto);
                var totalIngresos = ingresos.Where(i => i.Fecha.Month == mes).Sum(i => i.Monto);

                resultado.Add(new BalanceHistoricoItem
                {
                    Mes = mes,
                    TotalGastos = totalGastos,
                    TotalIngresos = totalIngresos
                });
            }

            return resultado;
        }
    }
}
