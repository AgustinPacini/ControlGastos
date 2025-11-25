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
    public class GetIngresoByIdQueryHandler : IRequestHandler<GetIngresoByIdQuery, Ingresos?>
    {
        private readonly IBaseRepository<Ingresos> _baseRepository;

        public GetIngresoByIdQueryHandler(IBaseRepository<Ingresos> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<Ingresos?> Handle(GetIngresoByIdQuery request, CancellationToken cancellationToken)
        {
            var ingreso = await _baseRepository.GetById(request.Id);
            if (ingreso == null || ingreso.UsuarioId != request.UsuarioId)
                return null;

            return ingreso;
        }
    }
}
