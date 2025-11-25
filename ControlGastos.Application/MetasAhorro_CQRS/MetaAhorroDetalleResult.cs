using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.MetasAhorro_CQRS
{
    public class MetaAhorroDetalleResult
    {
        public int Id { get; set; }
        public string NombreObjetivo { get; set; } = string.Empty;
        public decimal MontoObjetivo { get; set; }
        public decimal MontoAhorrado { get; set; }
        public decimal MontoRestante => MontoObjetivo - MontoAhorrado;
        public DateTime FechaObjetivo { get; set; }
        public int MesesRestantes { get; set; }
        public decimal AporteMensualSugerido { get; set; }
        public decimal PorcentajeAvance { get; set; }
    }
}
