using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Entity
{
    public class GastoFijo
    {
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;  // Ej: "Alquiler", "Netflix"
        public decimal Monto { get; set; }

        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Periodicidad, ejemplo: "Mensual", "Semanal", "Anual".
        /// </summary>
        public string Periodicidad { get; set; } = "Mensual";

        /// <summary>
        /// Día del mes en el que se considera el gasto (1-31).
        /// Para semanal podrías ignorarlo o usarlo como día de la semana.
        /// </summary>
        public int DiaReferencia { get; set; }

        public bool Activo { get; set; } = true;

        // Multiusuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // Opcional: asociarlo a categoría y cuenta
        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public int? CuentaId { get; set; }
        public Cuenta? Cuenta { get; set; }
    }
}
