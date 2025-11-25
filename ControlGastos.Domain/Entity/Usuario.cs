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

        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
        public ICollection<Ingresos> Ingresos { get; set; } = new List<Ingresos>();
        public ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();
        public ICollection<MetaAhorro> MetasAhorro { get; set; } = new List<MetaAhorro>();
        public ICollection<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
        public ICollection<GastoFijo> GastosFijos { get; set; } = new List<GastoFijo>();
    }
}
