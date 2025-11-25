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
    public class GetGastoByIdQueryHandler : IRequestHandler<GetGastoByIdQuery, Gasto?>
    {
        private readonly IBaseRepository<Gasto> _baseRepository;

        public GetGastoByIdQueryHandler(IBaseRepository<Gasto> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<Gasto?> Handle(GetGastoByIdQuery request, CancellationToken cancellationToken)
        {
            var gasto = await _baseRepository.GetById(request.Id);
            if (gasto == null || gasto.UsuarioId != request.UsuarioId)
                return null;

            return gasto;
        }
    }
}
