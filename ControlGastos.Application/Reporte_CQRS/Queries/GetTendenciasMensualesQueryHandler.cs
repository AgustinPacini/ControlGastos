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
    public class GetTendenciasMensualesQueryHandler
       : IRequestHandler<GetTendenciasMensualesQuery, List<TendenciaMensualItem>>
    {
        private readonly IBaseRepository<Gasto> _gastoRepo;
        private readonly IBaseRepository<Ingresos> _ingresoRepo;

        public GetTendenciasMensualesQueryHandler(
            IBaseRepository<Gasto> gastoRepo,
            IBaseRepository<Ingresos> ingresoRepo)
        {
            _gastoRepo = gastoRepo;
            _ingresoRepo = ingresoRepo;
        }

        public async Task<List<TendenciaMensualItem>> Handle(GetTendenciasMensualesQuery request, CancellationToken cancellationToken)
        {
            // Usamos una variable local en lugar de modificar la propiedad del record
            var meses = request.Meses <= 0 ? 6 : request.Meses;

            var hoy = DateTime.Today;
            var desde = hoy.AddMonths(-meses + 1);

            var gastos = (await _gastoRepo.GetAllAsync())
                .Where(g => g.UsuarioId == request.UsuarioId
                            && g.Fecha.Date >= desde.Date
                            && g.Fecha.Date <= hoy)
                .ToList();

            var ingresos = (await _ingresoRepo.GetAllAsync())
                .Where(i => i.UsuarioId == request.UsuarioId
                            && i.Fecha.Date >= desde.Date
                            && i.Fecha.Date <= hoy)
                .ToList();

            var resultados = new List<TendenciaMensualItem>();

            for (int offset = meses - 1; offset >= 0; offset--)
            {
                var fecha = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(-offset);
                var anio = fecha.Year;
                var mes = fecha.Month;

                var totalGastos = gastos.Where(g => g.Fecha.Year == anio && g.Fecha.Month == mes).Sum(g => g.Monto);
                var totalIngresos = ingresos.Where(i => i.Fecha.Year == anio && i.Fecha.Month == mes).Sum(i => i.Monto);

                resultados.Add(new TendenciaMensualItem
                {
                    Anio = anio,
                    Mes = mes,
                    TotalGastos = totalGastos,
                    TotalIngresos = totalIngresos
                });
            }

            return resultados;
        }
    }
}
