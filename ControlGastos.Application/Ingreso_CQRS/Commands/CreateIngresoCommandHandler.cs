using ControlGastos.Application.Gasto_CQRS.Commands;
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
    public class CreateIngresoCommandHandler: IRequestHandler<CreateIngresoCommand, int>
    {
        
        private readonly Domain.Interfaces.IBaseRepository<Domain.Entity.Ingresos> _baseRepository;

        public CreateIngresoCommandHandler( Domain.Interfaces.IBaseRepository<Domain.Entity.Ingresos> baseRepository)
        {
            
            _baseRepository = baseRepository;
        }

        public async Task<int> Handle(CreateIngresoCommand request, CancellationToken cancellationToken)
        {
            // Validaciones de negocio
            if (request.Ingresos.Monto <= 0)
                throw new Exception("El monto debe ser mayor a cero.");

            // Creamos la entidad Gasto
            var ingresos = new Domain.Entity.Ingresos
            {
                Fuente = request.Ingresos.Fuente,
                Monto = request.Ingresos.Monto,
                Fecha = request.Ingresos.Fecha,
                MetodoRecepcion = request.Ingresos.MetodoRecepcion
            };

            // Guardamos en el repositorio
            await _baseRepository.AddAsync(ingresos);

            // Retornamos el Id (asumiendo que la DB lo generó)
            return ingresos.Id;
        }
    }
}
