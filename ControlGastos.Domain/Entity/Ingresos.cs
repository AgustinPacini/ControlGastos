using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Entity
{
    public class Ingresos
    {
        public int Id { get; set; }
        public string Fuente { get; set; }
        public int Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string MetodoRecepcion { get; set; }
        public string Notas { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
    }
}
