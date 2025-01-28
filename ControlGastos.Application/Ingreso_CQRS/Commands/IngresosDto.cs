using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Ingreso_CQRS.Commands
{
    public class IngresosDto
    {
        public string Fuente { get; set; }
        public int Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string MetodoRecepcion { get; set; }
        public string Notas { get; set; }
    }
}
