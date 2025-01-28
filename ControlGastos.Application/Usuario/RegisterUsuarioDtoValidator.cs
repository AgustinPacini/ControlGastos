using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Usuario
{
    public class RegisterUsuarioDtoValidator : AbstractValidator<RegisterUsuarioDto>
    {
        public RegisterUsuarioDtoValidator()
        {
            RuleFor(x => x.NombreUsuario)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MinimumLength(4).WithMessage("El nombre de usuario debe tener al menos 4 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("Debe ser un email válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}
