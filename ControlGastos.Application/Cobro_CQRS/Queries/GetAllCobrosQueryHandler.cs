using ControlGastos.Application.Gasto_CQRS.Queries;
using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cobro_CQRS.Queries
{
    public class GetAllCobrosQueryHandler
    {
        private readonly ICobroRepository _cobroRepository;
        private readonly Domain.Interfaces.IBaseRepository<Domain.Entity.Cobro> _baseRepository;

        public GetAllCobrosQueryHandler(ICobroRepository gastoRepository, Domain.Interfaces.IBaseRepository<Domain.Entity.Cobro> baseRepository)
        {
            _cobroRepository = gastoRepository;
            _baseRepository = baseRepository;
        }

        public async Task<List<Gasto>> Handle(GetAllGastosQuery request, CancellationToken cancellationToken)
        {
            return (List<Gasto>)await _baseRepository.GetAllAsync();
        }
    }
}
