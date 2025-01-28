using ControlGastos.Domain.Interfaces;
using ControlGastos.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace ControlGastos.Application.Gasto_CQRS.Commands
{
    public class CreateGastoCommandHandler : IRequestHandler<CreateGastoCommand, int>
    {
        
        private readonly Domain.Interfaces.IBaseRepository<Domain.Entity.Gasto> _baseRepository;
        private readonly IValidator<CreateGastoDto> _validator;

        public CreateGastoCommandHandler(Domain.Interfaces.IBaseRepository<Domain.Entity.Gasto> baseRepository, IValidator<CreateGastoDto> validator)
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
                // Retornar error o lanzar excepción
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                throw new Exception($"Errores de validación: {string.Join(", ", errors)}");
            }
            // 2. Mapear DTO -> Entidad de Dominio
            var gasto = new Gasto
            {
                Descripcion = request.GastoDto.Concepto,
                Monto = request.GastoDto.Monto,
                Fecha = request.GastoDto.Fecha,
                MetodoPago = request.GastoDto.MetodoPago,
                Notas = request.GastoDto.Notas
            };

            // 3. Guardar en repositorio
            await _baseRepository.AddAsync(gasto);

            // 4. Retornar Id
            return gasto.Id;
        }
    }
}
