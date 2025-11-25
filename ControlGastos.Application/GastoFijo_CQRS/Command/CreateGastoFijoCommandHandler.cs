using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.GastoFijo_CQRS.Command
{
    public class CreateGastoFijoCommandHandler
        : IRequestHandler<CreateGastoFijoCommand, int>
    {
        private readonly IBaseRepository<GastoFijo> _repo;

        public CreateGastoFijoCommandHandler(IBaseRepository<GastoFijo> repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreateGastoFijoCommand request, CancellationToken cancellationToken)
        {
            var entity = new GastoFijo
            {
                Descripcion = request.Gasto.Descripcion,
                Monto = request.Gasto.Monto,
                FechaInicio = request.Gasto.FechaInicio,
                Periodicidad = request.Gasto.Periodicidad,
                DiaReferencia = request.Gasto.DiaReferencia,
                Activo = request.Gasto.Activo,
                CategoriaId = request.Gasto.CategoriaId,
                CuentaId = request.Gasto.CuentaId,
                UsuarioId = request.UsuarioId
            };

            await _repo.AddAsync(entity);
            return entity.Id;
        }
    }
}
