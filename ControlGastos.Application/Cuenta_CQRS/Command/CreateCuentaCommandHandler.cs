using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cuenta_CQRS.Command
{
    public class CreateCuentaCommandHandler : IRequestHandler<CreateCuentaCommand, int>
    {
        private readonly IBaseRepository<Cuenta> _repo;

        public CreateCuentaCommandHandler(IBaseRepository<Cuenta> repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreateCuentaCommand request, CancellationToken cancellationToken)
        {
            var cuenta = new Cuenta
            {
                Nombre = request.Cuenta.Nombre,
                Tipo = request.Cuenta.Tipo,
                SaldoInicial = request.Cuenta.SaldoInicial,
                SaldoActual = request.Cuenta.SaldoActual,
                UsuarioId = request.UsuarioId
            };

            await _repo.AddAsync(cuenta, cancellationToken);
            return cuenta.Id;
        }
    }
}
