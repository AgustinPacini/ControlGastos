using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Presupuesto_CQRS.Queries
{
    public class PresupuestoDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Nombre del presupuesto (ej. "Presupuesto mensual general").
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Monto límite asignado al presupuesto.
        /// </summary>
        public decimal MontoLimite { get; set; }

        /// <summary>
        /// Mes al que aplica el presupuesto (1-12).
        /// </summary>
        public int Mes { get; set; }

        /// <summary>
        /// Año al que aplica el presupuesto.
        /// </summary>
        public int Anio { get; set; }

        /// <summary>
        /// Id de la categoría asociada (opcional).
        /// Si el presupuesto no es por categoría, puede ser null.
        /// </summary>
        public int? CategoriaId { get; set; }

        /// <summary>
        /// Nombre de la categoría asociada (opcional).
        /// </summary>
        public string? CategoriaNombre { get; set; }

        public int UsuarioId { get; set; }
    }
}
