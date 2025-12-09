using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Exceptions;
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
        private readonly IBaseRepository<Gasto> _gastoRepository;
        private readonly IValidator<GastoDto> _validator;
        private readonly IBaseRepository<Cuenta> _cuentaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGastoCommandHandler(IBaseRepository<Gasto> baseRepository,
                                         IValidator<GastoDto> validator, IBaseRepository<Cuenta> cuentaRepository, IUnitOfWork unitOfWork) 
        {
            _gastoRepository = baseRepository;
            _validator = validator;
            _cuentaRepository = cuentaRepository;
            _unitOfWork = unitOfWork;

        }

        public async Task<int> Handle(CreateGastoCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.GastoDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
            int nuevoGastoId = 0;

            // 2) Transacción: crear gasto + actualizar cuenta (si corresponde)
            await _unitOfWork.ExecuteInTransactionAsync(async ct =>
            {
                // 2.1) Si hay cuenta asociada, validamos existencia y pertenencia ANTES
                Cuenta? cuenta = null;

                if (request.GastoDto.CuentaId.HasValue)
                {
                    cuenta = await _cuentaRepository.GetByIdAsync(
                        request.GastoDto.CuentaId.Value,
                        ct);

                    if (cuenta == null)
                    {
                        throw new KeyNotFoundException("No se encontró la cuenta especificada.");
                    }

                    if (cuenta.UsuarioId != request.UsuarioId)
                    {
                        throw new ForbiddenAccessException("La cuenta no pertenece al usuario autenticado.");
                    }

                    // No permitir saldo negativo
                    if (cuenta.SaldoActual < request.GastoDto.Monto)
                    {
                        throw new ValidationException("La cuenta no tiene saldo suficiente para registrar este gasto.");
                    }
                }

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

                await _gastoRepository.AddAsync(gasto, ct);
                nuevoGastoId = gasto.Id;

                // 2.3) Si hay cuenta asociada, descontar saldo
                if (cuenta != null)
                {
                    cuenta.SaldoActual -= request.GastoDto.Monto;
                    await _cuentaRepository.UpdateAsync(cuenta, ct);
                }

            }, cancellationToken);

            return nuevoGastoId;
        }
    }
}

