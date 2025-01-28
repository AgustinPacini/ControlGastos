using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Commands
{
    public class CreateGastoDtoValidator : AbstractValidator<CreateGastoDto>
    {
        public CreateGastoDtoValidator()
        {
            RuleFor(x => x.Concepto)
                .NotEmpty().WithMessage("El concepto es obligatorio.")
                .MaximumLength(100).WithMessage("El concepto no puede superar los 100 caracteres.");

            RuleFor(x => x.Monto)
                .NotEmpty().WithMessage("El Monto es obligatorio.")
                .GreaterThan(0).WithMessage("El monto debe ser mayor que cero.");

            RuleFor(x => x.Fecha)
                .NotEmpty().WithMessage("La fecha es obligatorio.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha no puede ser futura.");

            RuleFor(x => x.MetodoPago)
                .NotEmpty().WithMessage("El método de pago es obligatorio.")
                .MaximumLength(50).WithMessage("El método de pago no puede superar los 50 caracteres.");

            // 'Notas' es opcional, pero podrías limitar su longitud
            RuleFor(x => x.Notas)
                .MaximumLength(250).WithMessage("Las notas no pueden superar los 250 caracteres.");
        }
    }
}
