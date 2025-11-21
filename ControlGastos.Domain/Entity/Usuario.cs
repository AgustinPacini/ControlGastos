using System;

namespace ControlGastos.Domain.Entity
{
    /// <summary>
    /// Representa un usuario del sistema de Control de Gastos.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de usuario utilizado para autenticarse.
        /// </summary>
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash de la contraseña (nunca almacenar el texto plano).
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;
    }
}
