using ControlGastos.Application.Gasto_CQRS.Commands;
using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cobro_CQRS.Commands
{
    public class CreateCobroCommandHandler
    {
        private readonly ICobroRepository _cobroRepository;
        private readonly Domain.Interfaces.IBaseRepository<Domain.Entity.Cobro> _baseRepository;

        public CreateCobroCommandHandler(ICobroRepository cobroRepository, Domain.Interfaces.IBaseRepository<Domain.Entity.Cobro> baseRepository)
        {
            _cobroRepository = cobroRepository;
            _baseRepository = baseRepository;
        }

        public async Task<int> Handle(CreateGastoCommand request, CancellationToken cancellationToken)
        {
            // Validaciones de negocio
            if (request.Monto <= 0)
                throw new Exception("El monto debe ser mayor a cero.");

            // Creamos la entidad Gasto
            var cobro = new Domain.Entity.Cobro
            {
                Descripcion = request.Descripcion,
                Monto = request.Monto,
                Fecha = request.Fecha,
                Tipo = (Cobro.TipoCobro)request.TipoGasto
            };

            // Guardamos en el repositorio
            await _baseRepository.AddAsync(cobro);

            // Retornamos el Id (asumiendo que la DB lo generó)
            return cobro.Id;
        }
    }
}
