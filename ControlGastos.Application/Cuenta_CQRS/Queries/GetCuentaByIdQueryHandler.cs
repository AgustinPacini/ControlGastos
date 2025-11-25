using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cuenta_CQRS.Queries
{
    public class GetCuentaByIdQueryHandler
        : IRequestHandler<GetCuentaByIdQuery, Cuenta?>
    {
        private readonly IBaseRepository<Cuenta> _repo;

        public GetCuentaByIdQueryHandler(IBaseRepository<Cuenta> repo)
        {
            _repo = repo;
        }

        public async Task<Cuenta?> Handle(GetCuentaByIdQuery request, CancellationToken cancellationToken)
        {
            var cuenta = await _repo.GetById(request.Id);
            if (cuenta == null || cuenta.UsuarioId != request.UsuarioId)
                return null;

            return cuenta;
        }
    }
}
