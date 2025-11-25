using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Commands
{
    public class DeleteGastoCommandHandler : IRequestHandler<DeleteGastoCommand, bool>
    {
        private readonly IBaseRepository<Gasto> _baseRepository;

        public DeleteGastoCommandHandler(IBaseRepository<Gasto> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<bool> Handle(DeleteGastoCommand request, CancellationToken cancellationToken)
        {
            var gasto = await _baseRepository.GetById(request.Id);
            if (gasto == null) return false;

            await _baseRepository.DeleteAsync(gasto);
            return true;
        }
    }
}
