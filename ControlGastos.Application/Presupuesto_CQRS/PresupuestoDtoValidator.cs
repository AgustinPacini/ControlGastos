using ControlGastos.Application.Presupuesto_CQRS.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Presupuesto_CQRS
{
    /// <summary>
    /// Validador para PresupuestoDto usado al crear/actualizar presupuestos.
    /// </summary>
    public class PresupuestoDtoValidator : AbstractValidator<PresupuestoDto>
    {
        public PresupuestoDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del presupuesto es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(x => x.MontoLimite)
                .GreaterThan(0).WithMessage("El monto límite debe ser mayor que 0.");

            RuleFor(x => x.Mes)
                .InclusiveBetween(1, 12).WithMessage("El mes debe estar entre 1 y 12.");

            RuleFor(x => x.Anio)
                .InclusiveBetween(DateTime.Now.Year - 5, DateTime.Now.Year + 5)
                .WithMessage("El año del presupuesto es inválido.");

            // Si querés exigir categoría:
            // RuleFor(x => x.CategoriaId)
            //     .NotNull().WithMessage("La categoría es obligatoria.");
        }
    }
}

