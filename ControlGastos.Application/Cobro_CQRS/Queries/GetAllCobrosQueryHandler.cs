using ControlGastos.Application.Gasto_CQRS.Queries;
using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cobro_CQRS.Queries
{
    public class GetAllCobrosQueryHandler : IRequestHandler<GetAllCobrosQuery, List<Cobro>>
    {
        private readonly ICobroRepository _cobroRepository;
        private readonly Domain.Interfaces.IBaseRepository<Cobro> _baseRepository;

        public GetAllCobrosQueryHandler(ICobroRepository cobroRepository, Domain.Interfaces.IBaseRepository<Domain.Entity.Cobro> baseRepository)
        {
            _cobroRepository = cobroRepository;
            _baseRepository = baseRepository;
        }

        public async Task<List<Cobro>> Handle(GetAllCobrosQuery request, CancellationToken cancellationToken)
        {
            return (List<Cobro>)await _baseRepository.GetAllAsync();
        }
    }
}
