using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Ingreso_CQRS.Commands
{
   
        public class DeleteIngresoCommandHandler : IRequestHandler<DeleteIngresoCommand, bool>
        {
            private readonly IBaseRepository<Ingresos> _baseRepository;

            public DeleteIngresoCommandHandler(IBaseRepository<Ingresos> baseRepository)
            {
                _baseRepository = baseRepository;
            }

            public async Task<bool> Handle(DeleteIngresoCommand request, CancellationToken cancellationToken)
            {
                // 1. Buscamos el Gasto por Id
                var ingresos = await _baseRepository.GetById(request.Id);
                if (ingresos == null)
                {
                    // Si no existe, podrías lanzar una excepción 
                    // o retornar false, depende de tu preferencia
                    return false;
                }

                // 2. Eliminamos el gasto
                await _baseRepository.DeleteAsync(ingresos);

                return true;
            }
        }
    
}
