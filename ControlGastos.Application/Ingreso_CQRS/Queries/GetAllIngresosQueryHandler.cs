using ControlGastos.Application.Gasto_CQRS.Queries;
using ControlGastos.Application.Ingreso_CQRS.Commands;
using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Ingreso_CQRS.Queries
{
    public class GetAllIngresosQueryHandler : IRequestHandler<GetAllIngresosQuery, List<IngresosDto>>
    {
       
        private readonly Domain.Interfaces.IBaseRepository<Ingresos> _baseRepository;

        public GetAllIngresosQueryHandler( Domain.Interfaces.IBaseRepository<Domain.Entity.Ingresos> baseRepository)
        {
            
            _baseRepository = baseRepository;
        }

        public async Task<List<IngresosDto>> Handle(GetAllIngresosQuery request, CancellationToken cancellationToken)
        {
            return (List<IngresosDto>)await _baseRepository.GetAllAsync();
        }
    }
}
