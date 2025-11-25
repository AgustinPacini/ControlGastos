using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.MetasAhorro_CQRS.Command
{
    public class UpdateMetaAhorroCommandHandler : IRequestHandler<UpdateMetaAhorroCommand, bool>
    {
        private readonly IBaseRepository<MetaAhorro> _metaRepo;

        public UpdateMetaAhorroCommandHandler(IBaseRepository<MetaAhorro> metaRepo)
        {
            _metaRepo = metaRepo;
        }

        public async Task<bool> Handle(UpdateMetaAhorroCommand request, CancellationToken cancellationToken)
        {
            var meta = await _metaRepo.GetById(request.Id);
            if (meta == null || meta.UsuarioId != request.UsuarioId)
                return false;

            meta.NombreObjetivo = request.Meta.NombreObjetivo;
            meta.MontoObjetivo = request.Meta.MontoObjetivo;
            meta.MontoAhorrado = request.Meta.MontoAhorrado;
            meta.FechaObjetivo = request.Meta.FechaObjetivo;

            await _metaRepo.UpdateAsync(meta);
            return true;
        }
    }
}
