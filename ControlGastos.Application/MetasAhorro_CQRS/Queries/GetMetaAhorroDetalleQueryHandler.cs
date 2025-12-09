using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.MetasAhorro_CQRS.Queries
{
    public class GetMetaAhorroDetalleQueryHandler
         : IRequestHandler<GetMetaAhorroDetalleQuery, MetaAhorroDetalleResult>
    {
        private readonly IBaseRepository<MetaAhorro> _repo;

        public GetMetaAhorroDetalleQueryHandler(IBaseRepository<MetaAhorro> repo)
        {
            _repo = repo;
        }

        public async Task<MetaAhorroDetalleResult> Handle(
    GetMetaAhorroDetalleQuery request, CancellationToken cancellationToken)
        {
            var meta = await _repo.GetById(request.Id, cancellationToken);

            if (meta == null)
                throw new KeyNotFoundException("Meta de ahorro no encontrada.");

            var hoy = DateTime.Today;

            var mesesRestantes = ((meta.FechaObjetivo.Year - hoy.Year) * 12)
                                + meta.FechaObjetivo.Month - hoy.Month;
            if (mesesRestantes <= 0) mesesRestantes = 1;

            var montoRestante = meta.MontoObjetivo - meta.MontoAhorrado;
            if (montoRestante < 0) montoRestante = 0;

            var aporteMensual = montoRestante / mesesRestantes;

            var diasTotalesRestantes = (meta.FechaObjetivo.Date - hoy.Date).TotalDays;
            if (diasTotalesRestantes <= 0) diasTotalesRestantes = 1;

            var semanasRestantes = Math.Ceiling(diasTotalesRestantes / 7d); // 👈 acá el cambio

            var aporteSemanal = montoRestante / (decimal)semanasRestantes;
            var aporteDiario = montoRestante / (decimal)diasTotalesRestantes;

            return new MetaAhorroDetalleResult
            {
                Id = meta.Id,
                NombreObjetivo = meta.NombreObjetivo,
                MontoObjetivo = meta.MontoObjetivo,
                MontoAhorrado = meta.MontoAhorrado,
                FechaObjetivo = meta.FechaObjetivo,
                MesesRestantes = mesesRestantes,
                AporteMensualSugerido = Math.Round(aporteMensual, 2),
                AporteSemanalSugerido = Math.Round(aporteSemanal, 2),
                AporteDiarioSugerido = Math.Round(aporteDiario, 2),
                PorcentajeAvance = meta.MontoObjetivo == 0
                    ? 0
                    : Math.Round((meta.MontoAhorrado / meta.MontoObjetivo) * 100, 2)
            };
        }
    }
}
