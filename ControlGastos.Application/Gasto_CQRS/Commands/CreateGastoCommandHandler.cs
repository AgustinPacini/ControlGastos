using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Commands
{
    /// <summary>
    /// Handler encargado de crear un nuevo gasto.
    /// </summary>
    public class CreateGastoCommandHandler : IRequestHandler<CreateGastoCommand, int>
    {
        private readonly IBaseRepository<Gasto> _baseRepository;
        private readonly IValidator<GastoDto> _validator;

        public CreateGastoCommandHandler(IBaseRepository<Gasto> baseRepository,
                                         IValidator<GastoDto> validator)
        {
            _baseRepository = baseRepository;
            _validator = validator;
        }

        public async Task<int> Handle(CreateGastoCommand request, CancellationToken cancellationToken)
        {
            // 1. Validar DTO con FluentValidation
            var validationResult = await _validator.ValidateAsync(request.GastoDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // 2. Mapear DTO -> Entidad de dominio
            var gasto = new Gasto
            {
                Descripcion = request.GastoDto.Concepto ?? string.Empty,
                Monto = request.GastoDto.Monto,
                Fecha = request.GastoDto.Fecha,
                MetodoPago = request.GastoDto.MetodoPago ?? string.Empty,
                Notas = request.GastoDto.Notas,
                CategoriaId = request.GastoDto.CategoriaId
            };

            // 3. Guardar en repositorio
            await _baseRepository.AddAsync(gasto);

            // 4. Retornar Id
            return gasto.Id;
        }
    }
}
