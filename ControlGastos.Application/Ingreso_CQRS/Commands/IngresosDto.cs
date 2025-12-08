using System;

namespace ControlGastos.Application.Ingreso_CQRS.Commands
{
    /// <summary>
    /// DTO utilizado para la creación y consulta de ingresos desde la capa de aplicación / API.
    /// Mantiene desacoplada la entidad de dominio de los contratos expuestos al exterior.
    /// </summary>
    public class IngresosDto
    {
         public int Id { get; set; }
        /// <summary>
        /// Origen del ingreso (por ejemplo: salario, freelance, venta, etc.).
        /// </summary>
        public string Fuente { get; set; } = string.Empty;

        /// <summary>
        /// Importe del ingreso.
        /// </summary>
        public decimal Monto { get; set; }

        /// <summary>
        /// Fecha en la que se produjo o registró el ingreso.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Método por el cual se recibió el dinero (efectivo, transferencia, billetera virtual, etc.).
        /// </summary>
        public string MetodoRecepcion { get; set; } = string.Empty;

        /// <summary>
        /// Notas adicionales u observaciones (opcional).
        /// </summary>
        public string? Notas { get; set; }

        public int CategoriaId { get; set; }
        public int? CuentaId { get; set; }
    }
}
