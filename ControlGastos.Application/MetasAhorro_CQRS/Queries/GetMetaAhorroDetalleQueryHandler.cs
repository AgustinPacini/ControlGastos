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
            var meta = await _repo.GetById(request.Id);

            if (meta == null)
                throw new Exception("Meta de ahorro no encontrada.");

            var hoy = DateTime.Today;

            var mesesRestantes = ((meta.FechaObjetivo.Year - hoy.Year) * 12)
                                + meta.FechaObjetivo.Month - hoy.Month;
            if (mesesRestantes <= 0) mesesRestantes = 1; // para evitar división por cero

            var montoRestante = meta.MontoObjetivo - meta.MontoAhorrado;
            if (montoRestante < 0) montoRestante = 0;

            var aporteMensual = montoRestante / mesesRestantes;

            return new MetaAhorroDetalleResult
            {
                Id = meta.Id,
                NombreObjetivo = meta.NombreObjetivo,
                MontoObjetivo = meta.MontoObjetivo,
                MontoAhorrado = meta.MontoAhorrado,
                FechaObjetivo = meta.FechaObjetivo,
                MesesRestantes = mesesRestantes,
                AporteMensualSugerido = Math.Round(aporteMensual, 2),
                PorcentajeAvance = meta.MontoObjetivo == 0
                    ? 0
                    : Math.Round((meta.MontoAhorrado / meta.MontoObjetivo) * 100, 2)
            };
        }
    }
}
