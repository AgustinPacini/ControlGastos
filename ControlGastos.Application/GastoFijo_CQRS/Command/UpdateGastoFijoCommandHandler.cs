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
    public class UpdateGastoFijoCommandHandler
       : IRequestHandler<UpdateGastoFijoCommand, bool>
    {
        private readonly IBaseRepository<GastoFijo> _repo;

        public UpdateGastoFijoCommandHandler(IBaseRepository<GastoFijo> repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(UpdateGastoFijoCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetById(request.Id);
            if (entity == null || entity.UsuarioId != request.UsuarioId)
                return false;

            entity.Descripcion = request.Gasto.Descripcion;
            entity.Monto = request.Gasto.Monto;
            entity.FechaInicio = request.Gasto.FechaInicio;
            entity.Periodicidad = request.Gasto.Periodicidad;
            entity.DiaReferencia = request.Gasto.DiaReferencia;
            entity.Activo = request.Gasto.Activo;
            entity.CategoriaId = request.Gasto.CategoriaId;
            entity.CuentaId = request.Gasto.CuentaId;

            await _repo.UpdateAsync(entity);
            return true;
        }
    }
}
