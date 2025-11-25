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
    public class GetCuentasByUsuarioQueryHandler
        : IRequestHandler<GetCuentasByUsuarioQuery, List<Cuenta>>
    {
        private readonly IBaseRepository<Cuenta> _repo;

        public GetCuentasByUsuarioQueryHandler(IBaseRepository<Cuenta> repo)
        {
            _repo = repo;
        }

        public async Task<List<Cuenta>> Handle(GetCuentasByUsuarioQuery request, CancellationToken cancellationToken)
        {
            var cuentas = await _repo.GetAllAsync();
            return cuentas
                .Where(c => c.UsuarioId == request.UsuarioId)
                .OrderBy(c => c.Nombre)
                .ToList();
        }
    }
}
