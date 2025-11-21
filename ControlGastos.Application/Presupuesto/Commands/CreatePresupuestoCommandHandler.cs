using ControlGastos.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DomainPresupuesto = ControlGastos.Domain.Entity.Presupuesto;

namespace ControlGastos.Application.Presupuesto.Commands
{
    /// <summary>
    /// Handler encargado de crear un nuevo presupuesto.
    /// </summary>
    public class CreatePresupuestoCommandHandler : IRequestHandler<CreatePresupuestoCommand, int>
    {
        private readonly IBaseRepository<DomainPresupuesto> _presupuestoRepository;

        public CreatePresupuestoCommandHandler(IBaseRepository<DomainPresupuesto> presupuestoRepository)
        {
            _presupuestoRepository = presupuestoRepository;
        }

        public async Task<int> Handle(CreatePresupuestoCommand request, CancellationToken cancellationToken)
        {
            var presupuesto = new DomainPresupuesto
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
