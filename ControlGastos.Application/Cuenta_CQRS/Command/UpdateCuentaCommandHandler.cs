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
    public class UpdateCuentaCommandHandler : IRequestHandler<UpdateCuentaCommand, bool>
    {
        private readonly IBaseRepository<Cuenta> _repo;

        public UpdateCuentaCommandHandler(IBaseRepository<Cuenta> repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(UpdateCuentaCommand request, CancellationToken cancellationToken)
        {
            var cuenta = await _repo.GetById(request.Id);
            if (cuenta == null || cuenta.UsuarioId != request.UsuarioId)
                return false;

            cuenta.Nombre = request.Cuenta.Nombre;
            cuenta.Tipo = request.Cuenta.Tipo;
            cuenta.SaldoInicial = request.Cuenta.SaldoInicial;
            cuenta.SaldoActual = request.Cuenta.SaldoActual;

            await _repo.UpdateAsync(cuenta);
            return true;
        }
    }
}
