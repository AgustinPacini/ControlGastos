using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Entity
{
    public class Categoria
    {
        public int Id { get; set; }

        
        /// Nombre de la categoría (ej. "Alimentos", "Transporte", etc.)
       
        public string Nombre { get; set; } = string.Empty;

       
        /// Descripción adicional de la categoría (opcional)
        
        public string? Descripcion { get; set; }

        // Relación con Gastos e Ingresos (opcional si decides categorizar ambos)
        public ICollection<Gasto>? Gastos { get; set; }
        public ICollection<Ingresos>? Ingresos { get; set; }
    }
}
