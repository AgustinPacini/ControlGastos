using ControlGastos.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DomainUsuario = ControlGastos.Domain.Entity.Usuario;

namespace ControlGastos.Application.Usuario
{
    /// <summary>
    /// Handler encargado de registrar un nuevo usuario en el sistema.
    /// </summary>
    public class RegisterUsuarioCommandHandler : IRequestHandler<RegisterUsuarioCommand, int>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IBaseRepository<DomainUsuario> _baseRepository;
        private readonly IValidator<RegisterUsuarioDto> _validator;

        public RegisterUsuarioCommandHandler(
            IUsuarioRepository usuarioRepository,
            IBaseRepository<DomainUsuario> baseRepository,
            IValidator<RegisterUsuarioDto> validator)
        {
            _usuarioRepository = usuarioRepository;
            _baseRepository = baseRepository;
            _validator = validator;
        }

        public async Task<int> Handle(RegisterUsuarioCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UsuarioDto;

            // 1. Validar el DTO
            var validationResult = await _validator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // 2. Verificar si ya existe un usuario con ese nombre
            var existe = await _usuarioRepository.ExistsByNombreUsuarioAsync(dto.NombreUsuario);
            if (existe)
            {
                throw new ValidationException("El nombre de usuario ya está en uso.");
            }

            // 3. Hashear password (por ahora podrías integrar un servicio de hash específico)
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password); // TODO: reemplazar por hashing real

            // 4. Crear entidad de dominio
            var nuevoUsuario = new DomainUsuario
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
