using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Presupuesto_CQRS.Queries
{
    public class GetPresupuestosMensualesQueryHandler
         : IRequestHandler<GetPresupuestosMensualesQuery, List<PresupuestoMensualResult>>
    {
        private readonly IPresupuestoRepository _presupuestoRepo;
        private readonly IGastoRepository _gastoRepo;

        public GetPresupuestosMensualesQueryHandler(
            IPresupuestoRepository presupuestoRepo,
            IGastoRepository gastoRepo)
        {
            _presupuestoRepo = presupuestoRepo;
            _gastoRepo = gastoRepo;
        }

        public async Task<List<PresupuestoMensualResult>> Handle(
            GetPresupuestosMensualesQuery request,
            CancellationToken cancellationToken)
        {
            var presupuestos = await _presupuestoRepo.GetByMesAnioAsync(request.Anio, request.Mes);
            var gastosMes = await _gastoRepo.GetByMonth(request.Anio, request.Mes);

            var list = new List<PresupuestoMensualResult>();

            foreach (var p in presupuestos)
            {
                decimal gastado;

                if (p.CategoriaId.HasValue)
                {
                    // Presupuesto por categoría
                    gastado = gastosMes
                        .Where(g => g.CategoriaId == p.CategoriaId.Value)
                        .Sum(g => g.Monto);
                }
                else
                {
                    // Presupuesto general del mes
                    gastado = gastosMes.Sum(g => g.Monto);
                }

                list.Add(new PresupuestoMensualResult
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    MontoLimite = p.MontoLimite,
                    MontoGastado = gastado,
                    Mes = p.Mes,
                    Anio = p.Anio,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria?.Nombre
                });
            }

            return list;
        }
    }
}
