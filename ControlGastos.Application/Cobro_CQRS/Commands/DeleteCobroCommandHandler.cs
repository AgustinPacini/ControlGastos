using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cobro_CQRS.Commands
{
   
        public class DeleteCobroCommandHandler : IRequestHandler<DeleteCobroCommand, bool>
        {
            private readonly IBaseRepository<Cobro> _baseRepository;

            public DeleteCobroCommandHandler(IBaseRepository<Cobro> baseRepository)
            {
                _baseRepository = baseRepository;
            }

            public async Task<bool> Handle(DeleteCobroCommand request, CancellationToken cancellationToken)
            {
                // 1. Buscamos el Gasto por Id
                var cobro = await _baseRepository.GetById(request.Id);
                if (cobro == null)
                {
                    // Si no existe, podrías lanzar una excepción 
                    // o retornar false, depende de tu preferencia
                    return false;
                }

                // 2. Eliminamos el gasto
                await _baseRepository.DeleteAsync(cobro);

                return true;
            }
        }
    
}
