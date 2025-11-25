using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.GastoFijo_CQRS.Queries
{
    public class GetGastosFijosByUsuarioQueryHandler
        : IRequestHandler<GetGastosFijosByUsuarioQuery, List<GastoFijo>>
    {
        private readonly IBaseRepository<GastoFijo> _repo;

        public GetGastosFijosByUsuarioQueryHandler(IBaseRepository<GastoFijo> repo)
        {
            _repo = repo;
        }

        public async Task<List<GastoFijo>> Handle(GetGastosFijosByUsuarioQuery request, CancellationToken cancellationToken)
        {
            var data = await _repo.GetAllAsync();
            return data
                .Where(g => g.UsuarioId == request.UsuarioId)
                .OrderBy(g => g.Descripcion)
                .ToList();
        }
    }
}
