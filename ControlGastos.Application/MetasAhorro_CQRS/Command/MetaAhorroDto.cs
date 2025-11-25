using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.MetasAhorro_CQRS.Command
{
    public class MetaAhorroDto
    {
        public string NombreObjetivo { get; set; } = string.Empty;
        public decimal MontoObjetivo { get; set; }
        public decimal MontoAhorrado { get; set; }   // 0 al crear
        public DateTime FechaObjetivo { get; set; }
    }
}
