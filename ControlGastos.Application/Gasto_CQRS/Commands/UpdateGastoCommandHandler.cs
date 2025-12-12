using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Exceptions;
using ControlGastos.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Commands
{

    public class UpdateGastoCommandHandler : IRequestHandler<UpdateGastoCommand, bool>
    {
        private readonly IBaseRepository<Gasto> _gastoRepository;
        private readonly IBaseRepository<Cuenta> _cuentaRepository;
        private readonly IValidator<GastoDto> _validator;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGastoCommandHandler(IBaseRepository<Gasto> gastoRepository,
            IBaseRepository<Cuenta> cuentaRepository,
            IValidator<GastoDto> validator,
            IUnitOfWork unitOfWork)
        {
            _gastoRepository = gastoRepository;
            _cuentaRepository = cuentaRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateGastoCommand request, CancellationToken cancellationToken)
        {
            // 1) Validar DTO (mismo validator que en Create)
            var validationResult = await _validator.ValidateAsync(request.GastoDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var actualizado = false;

            await _unitOfWork.ExecuteInTransactionAsync(async ct =>
            {
                // 2) Obtener el gasto
                var gasto = await _gastoRepository.GetByIdAsync(request.Id, ct);
                if (gasto == null)
                {
                    // No existe → devolvemos false al controller
                    return;
                }

                // 3) Validar ownership del gasto
                if (gasto.UsuarioId != request.UsuarioId)
                    throw new ForbiddenAccessException("El gasto no pertenece al usuario autenticado.");

                // 4) Preparar datos de cuentas y montos
                var montoOriginal = gasto.Monto;
                var montoNuevo = request.GastoDto.Monto;

                var cuentaOriginalId = gasto.CuentaId;
                var nuevaCuentaId = request.GastoDto.CuentaId;

                Cuenta? cuentaOriginal = null;
                Cuenta? nuevaCuenta = null;

                // 4.1) Cargar cuenta original (si tenía)
                if (cuentaOriginalId.HasValue)
                {
                    cuentaOriginal = await _cuentaRepository.GetByIdAsync(cuentaOriginalId.Value, ct);
                    if (cuentaOriginal == null)
                        throw new KeyNotFoundException("La cuenta original asociada al gasto no existe.");

                    if (cuentaOriginal.UsuarioId != request.UsuarioId)
                        throw new ForbiddenAccessException("La cuenta original no pertenece al usuario autenticado.");
                }

                // 4.2) Cargar nueva cuenta (si se envía)
                if (nuevaCuentaId.HasValue)
                {
                    nuevaCuenta = await _cuentaRepository.GetByIdAsync(nuevaCuentaId.Value, ct);
                    if (nuevaCuenta == null)
                        throw new KeyNotFoundException("La cuenta seleccionada para el gasto no existe.");

                    if (nuevaCuenta.UsuarioId != request.UsuarioId)
                        throw new ForbiddenAccessException("La cuenta seleccionada no pertenece al usuario autenticado.");
                }

                // 5) Ajuste contable según el cambio de cuenta/monto
                if (cuentaOriginalId == nuevaCuentaId)
                {
                    // Misma cuenta: ajustar sólo la diferencia de monto
                    if (nuevaCuenta != null)
                    {
                        var diferencia = montoNuevo - montoOriginal;

                        if (diferencia > 0)
                        {
                            // Hay que descontar saldo adicional
                            if (nuevaCuenta.SaldoActual < diferencia)
                                throw new ValidationException("La cuenta no tiene saldo suficiente para actualizar el gasto.");

                            nuevaCuenta.SaldoActual -= diferencia;
                        }
                        else if (diferencia < 0)
                        {
                            // Se devuelve parte del monto a la cuenta
                            nuevaCuenta.SaldoActual += (-diferencia);
                        }
                    }
                }
                else
                {
                    // Cuenta distinta o se agrega/quita cuenta

                    // 5.1) Si había cuenta original → devolver el monto original
                    if (cuentaOriginal != null)
                    {
                        cuentaOriginal.SaldoActual += montoOriginal;
                    }

                    // 5.2) Si hay nueva cuenta → descontar el monto nuevo
                    if (nuevaCuenta != null)
                    {
                        if (nuevaCuenta.SaldoActual < montoNuevo)
                            throw new ValidationException("La cuenta seleccionada no tiene saldo suficiente para actualizar el gasto.");

                        nuevaCuenta.SaldoActual -= montoNuevo;
                    }
                }

                // 6) Actualizar campos del gasto
                gasto.Descripcion = request.GastoDto.Concepto ?? gasto.Descripcion;
                gasto.Monto = montoNuevo;
                gasto.Fecha = request.GastoDto.Fecha;
                gasto.MetodoPago = request.GastoDto.MetodoPago ?? gasto.MetodoPago;
                gasto.Notas = request.GastoDto.Notas;
                gasto.CategoriaId = request.GastoDto.CategoriaId;
                gasto.CuentaId = nuevaCuentaId;

                await _gastoRepository.UpdateAsync(gasto, ct);

                if (cuentaOriginal != null)
                    await _cuentaRepository.UpdateAsync(cuentaOriginal, ct);

                if (nuevaCuenta != null && nuevaCuenta.Id != cuentaOriginal?.Id)
                    await _cuentaRepository.UpdateAsync(nuevaCuenta, ct);

                actualizado = true;

            }, cancellationToken);

            return actualizado;
        }
    }
}
