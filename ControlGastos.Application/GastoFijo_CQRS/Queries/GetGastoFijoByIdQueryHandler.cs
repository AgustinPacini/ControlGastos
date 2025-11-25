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
    public class GetGastoFijoByIdQueryHandler
        : IRequestHandler<GetGastoFijoByIdQuery, GastoFijo?>
    {
        private readonly IBaseRepository<GastoFijo> _repo;

        public GetGastoFijoByIdQueryHandler(IBaseRepository<GastoFijo> repo)
        {
            _repo = repo;
        }

        public async Task<GastoFijo?> Handle(GetGastoFijoByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetById(request.Id);
            if (entity == null || entity.UsuarioId != request.UsuarioId)
                return null;

            return entity;
        }
    }
}
