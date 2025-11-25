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
    public class GetMetasAhorroByUsuarioQueryHandler
        : IRequestHandler<GetMetasAhorroByUsuarioQuery, List<MetaAhorro>>
    {
        private readonly IBaseRepository<MetaAhorro> _metaRepo;

        public GetMetasAhorroByUsuarioQueryHandler(IBaseRepository<MetaAhorro> metaRepo)
        {
            _metaRepo = metaRepo;
        }

        public async Task<List<MetaAhorro>> Handle(GetMetasAhorroByUsuarioQuery request, CancellationToken cancellationToken)
        {
            var metas = await _metaRepo.GetAllAsync();
            return metas
                .Where(m => m.UsuarioId == request.UsuarioId)
                .OrderBy(m => m.FechaObjetivo)
                .ToList();
        }
    }
}
