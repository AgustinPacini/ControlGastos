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
    public class DeleteCuentaCommandHandler : IRequestHandler<DeleteCuentaCommand, bool>
    {
        private readonly IBaseRepository<Cuenta> _repo;

        public DeleteCuentaCommandHandler(IBaseRepository<Cuenta> repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteCuentaCommand request, CancellationToken cancellationToken)
        {
            var cuenta = await _repo.GetById(request.Id);
            if (cuenta == null || cuenta.UsuarioId != request.UsuarioId)
                return false;

            await _repo.DeleteAsync(cuenta);
            return true;
        }
    }
}
