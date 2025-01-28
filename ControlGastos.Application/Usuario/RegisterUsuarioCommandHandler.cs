using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Usuario
{
    public class RegisterUsuarioCommandHandler : IRequestHandler<RegisterUsuarioCommand, int>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IValidator<RegisterUsuarioDto> _validator;
        private readonly Domain.Interfaces.IBaseRepository<Domain.Entity.Usuario> _baseRepository;

        public RegisterUsuarioCommandHandler(
            IUsuarioRepository usuarioRepository,
            IValidator<RegisterUsuarioDto> validator,
            IBaseRepository<ControlGastos.Domain.Entity.Usuario> baseRepository)
        {
            _usuarioRepository = usuarioRepository;
            _validator = validator;
            _baseRepository = baseRepository;
        }

        public async Task<int> Handle(RegisterUsuarioCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UsuarioDto;

            // 1. Validamos el DTO con FluentValidation
            var validationResult = await _validator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                throw new Exception("Errores de validación: " + string.Join(", ", errors));
            }

            // 2. Verificar si ya existe un usuario con el mismo NombreUsuario
            var existeUsuario = await _usuarioRepository.ExistsByNombreUsuarioAsync(dto.NombreUsuario);
            if (existeUsuario)
                throw new Exception("El nombre de usuario ya está en uso.");

            // 3. Hashear la contraseña con BCrypt
            // Por defecto, genera salt internamente y aplica Work Factor 10 (puedes ajustarlo).
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 4. Crear la entidad Usuario
            var nuevoUsuario = new ControlGastos.Domain.Entity.Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                Email = dto.Email,
                PasswordHash = passwordHash
            };

            // 5. Guardar en la base de datos
            await _baseRepository.AddAsync(nuevoUsuario);

            // 6. Retornar el ID
            return nuevoUsuario.Id;
        }
    }
}
