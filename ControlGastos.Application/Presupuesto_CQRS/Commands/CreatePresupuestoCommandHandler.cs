using ControlGastos.Application.Gasto_CQRS.Commands;
using ControlGastos.Application.Presupuesto_CQRS.Queries;
using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
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
        private readonly IValidator<PresupuestoDto> _validator;


        public CreatePresupuestoCommandHandler(IBaseRepository<DomainPresupuesto> presupuestoRepository, IValidator<PresupuestoDto> validator)
        {
            _presupuestoRepository = presupuestoRepository;
            _validator = validator;
        }

        public async Task<int> Handle(CreatePresupuestoCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.PresupuestoDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            var presupuesto = new DomainPresupuesto
            {
                Nombre = request.PresupuestoDto.Nombre,
                MontoLimite = request.PresupuestoDto.MontoLimite,
                Mes = request.PresupuestoDto.Mes,
                Anio = request.PresupuestoDto.Anio,
                CategoriaId = request.PresupuestoDto.CategoriaId,
                UsuarioId = request.UsuarioId
            };

            await _presupuestoRepository.AddAsync(presupuesto, cancellationToken);

            return presupuesto.Id;
        }
    }
}
