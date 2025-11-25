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
    public class DeleteMetaAhorroCommandHandler : IRequestHandler<DeleteMetaAhorroCommand, bool>
    {
        private readonly IBaseRepository<MetaAhorro> _metaRepo;

        public DeleteMetaAhorroCommandHandler(IBaseRepository<MetaAhorro> metaRepo)
        {
            _metaRepo = metaRepo;
        }

        public async Task<bool> Handle(DeleteMetaAhorroCommand request, CancellationToken cancellationToken)
        {
            var meta = await _metaRepo.GetById(request.Id);
            if (meta == null || meta.UsuarioId != request.UsuarioId)
                return false;

            await _metaRepo.DeleteAsync(meta);
            return true;
        }
    }
}
