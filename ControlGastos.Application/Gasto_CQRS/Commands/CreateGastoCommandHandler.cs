using ControlGastos.Domain.Interfaces;
using ControlGastos.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Commands
{
    public class CreateGastoCommandHandler : IRequestHandler<CreateGastoCommand, int>
    {
        private readonly IGastoRepository _gastoRepository;
        private readonly Domain.Interfaces.IBaseRepository<Domain.Entity.Gasto> _baseRepository;

        public CreateGastoCommandHandler(IGastoRepository gastoRepository,Domain.Interfaces.IBaseRepository<Domain.Entity.Gasto> baseRepository)
        {
            _gastoRepository = gastoRepository;
            _baseRepository = baseRepository;
        }

        public async Task<int> Handle(CreateGastoCommand request, CancellationToken cancellationToken)
        {
            // Validaciones de negocio
            if (request.Monto <= 0)
                throw new Exception("El monto debe ser mayor a cero.");

            // Creamos la entidad Gasto
            var gasto = new Domain.Entity.Gasto
            {
                Descripcion = request.Descripcion,
                Monto = request.Monto,
                Fecha = request.Fecha,
                Tipo = (Gasto.TipoGasto)request.TipoGasto
            };

            // Guardamos en el repositorio
            await _baseRepository.AddAsync(gasto);

            // Retornamos el Id (asumiendo que la DB lo generó)
            return gasto.Id;
        }
    }
}
