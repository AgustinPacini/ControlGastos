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
        private readonly IBaseRepository<Cuenta> _cuentaRepository;

        public CreateGastoCommandHandler(IBaseRepository<Gasto> baseRepository,
                                         IValidator<GastoDto> validator, IBaseRepository<Cuenta> cuentaRepository) 
        {
            _baseRepository = baseRepository;
            _validator = validator;
            _cuentaRepository = cuentaRepository;

        }

        public async Task<int> Handle(CreateGastoCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.GastoDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var gasto = new Gasto
            {
                Descripcion = request.GastoDto.Concepto ?? string.Empty,
                Monto = request.GastoDto.Monto,
                Fecha = request.GastoDto.Fecha,
                MetodoPago = request.GastoDto.MetodoPago ?? string.Empty,
                Notas = request.GastoDto.Notas,
                CategoriaId = request.GastoDto.CategoriaId,
                UsuarioId = request.UsuarioId,  // 🔹 importante
                 CuentaId = request.GastoDto.CuentaId
            };

            await _baseRepository.AddAsync(gasto);
            // 👇 Si el gasto tiene cuenta asociada, descontamos el saldo
            if (request.GastoDto.CuentaId.HasValue)
            {
                var cuenta = await _cuentaRepository.GetById(request.GastoDto.CuentaId.Value);

                if (cuenta.UsuarioId != request.UsuarioId)
                    throw new InvalidOperationException("La cuenta no pertenece al usuario.");

                cuenta.SaldoActual -= request.GastoDto.Monto;
                await _cuentaRepository.UpdateAsync(cuenta);
            }
            return gasto.Id;
        }
    }
}
