using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Entity
{
    public class Presupuesto
    {
        public int Id { get; set; }

        /// <summary>
        /// Nombre del presupuesto (ej. "Presupuesto Mensual")
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Monto límite del presupuesto
        /// </summary>
        public decimal MontoLimite { get; set; }

        /// <summary>
        /// Mes al que se aplica el presupuesto
        /// </summary>
        public int Mes { get; set; }

        /// <summary>
        /// Año al que se aplica el presupuesto
        /// </summary>
        public int Anio { get; set; }

        /// <summary>
        /// Relación con Categoria (opcional, si el presupuesto es por categoría)
        /// </summary>
        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        // 🔹 Multiusuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
