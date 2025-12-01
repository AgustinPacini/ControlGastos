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
    public class CreateMetaAhorroCommandHandler : IRequestHandler<CreateMetaAhorroCommand, int>
    {
        private readonly IBaseRepository<MetaAhorro> _metaRepo;

        public CreateMetaAhorroCommandHandler(IBaseRepository<MetaAhorro> metaRepo)
        {
            _metaRepo = metaRepo;
        }

        public async Task<int> Handle(CreateMetaAhorroCommand request, CancellationToken cancellationToken)
        {
            var meta = new MetaAhorro
            {
                NombreObjetivo = request.Meta.NombreObjetivo,
                MontoObjetivo = request.Meta.MontoObjetivo,
                MontoAhorrado = request.Meta.MontoAhorrado,
                FechaObjetivo = request.Meta.FechaObjetivo,
                FechaCreacion = DateTime.UtcNow,
                UsuarioId = request.UsuarioId
            };

            await _metaRepo.AddAsync(meta);
            return meta.Id;
        }
    }
}
