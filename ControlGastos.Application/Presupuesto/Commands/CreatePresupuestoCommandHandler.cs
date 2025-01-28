using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Presupuesto.Commands
{
    public class CreatePresupuestoCommandHandler : IRequestHandler<CreatePresupuestoCommand, int>
    {
        private readonly Domain.Interfaces.IBaseRepository<Domain.Entity.Presupuesto> _presupuestoRepository;
        

        public CreatePresupuestoCommandHandler(Domain.Interfaces.IBaseRepository<Domain.Entity.Presupuesto> presupuestoRepository)
        {
            _presupuestoRepository = presupuestoRepository;
        }

        public async Task<int> Handle(CreatePresupuestoCommand request, CancellationToken cancellationToken)
        {
            var presupuesto = new Domain.Entity.Presupuesto()
            {
                Nombre = request.Nombre,
                MontoLimite = request.MontoLimite,
                Mes = request.Mes,
                Anio = request.Anio,
                CategoriaId = request.CategoriaId
            };

            await _presupuestoRepository.AddAsync(presupuesto);

            return presupuesto.Id;
        }
    }
}
