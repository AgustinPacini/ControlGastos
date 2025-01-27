using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Queries
{
    public class GetAllGastosQueryHandler : IRequestHandler<GetAllGastosQuery, List<Gasto>>
    {
        private readonly IGastoRepository _gastoRepository;
        private readonly Domain.Interfaces.IBaseRepository<Domain.Entity.Gasto> _baseRepository;

        public GetAllGastosQueryHandler(IGastoRepository gastoRepository,Domain.Interfaces.IBaseRepository<Domain.Entity.Gasto> baseRepository)
        {
            _gastoRepository = gastoRepository;
            _baseRepository = baseRepository;
        }

        public async Task<List<Gasto>> Handle(GetAllGastosQuery request, CancellationToken cancellationToken)
        {
            return (List<Gasto>)await _baseRepository.GetAllAsync();
        }
    }
}
