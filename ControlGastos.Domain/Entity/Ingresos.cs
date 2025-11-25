using System;

namespace ControlGastos.Domain.Entity
{
    /// <summary>
    /// Representa un ingreso de dinero registrado en el sistema.
    /// </summary>
    public class Ingresos
    {
        /// <summary>
        /// Identificador único del ingreso.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Origen del ingreso (por ejemplo: salario, freelance, venta, etc.).
        /// </summary>
        public string Fuente { get; set; } = string.Empty;

        /// <summary>
        /// Importe del ingreso. Se utiliza decimal para evitar errores de redondeo en montos de dinero.
        /// </summary>
        public decimal Monto { get; set; }

        /// <summary>
        /// Fecha en la que se registró o se produjo el ingreso.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Método por el cual se recibió el dinero (efectivo, transferencia, billetera virtual, etc.).
        /// </summary>
        public string MetodoRecepcion { get; set; } = string.Empty;

        /// <summary>
        /// Notas adicionales u observaciones sobre el ingreso (opcional).
        /// </summary>
        public string? Notas { get; set; }

        /// <summary>
        /// Identificador de la categoría asociada al ingreso.
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// Categoría asociada al ingreso.
        /// </summary>
        public Categoria Categoria { get; set; } = null!;

        // 🔹 Multiusuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // 🔹 Relación con Cuenta
        public int? CuentaId { get; set; }
        public Cuenta? Cuenta { get; set; }
    }
}
