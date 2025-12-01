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

        // Lo que te falta ahorrar
        public decimal MontoRestante => MontoObjetivo - MontoAhorrado;

        public DateTime FechaObjetivo { get; set; }

        // Cuántos meses quedan hasta la fecha objetivo
        public int MesesRestantes { get; set; }

        // Plan sugerido
        public decimal AporteMensualSugerido { get; set; }
        public decimal AporteSemanalSugerido { get; set; }   // 🔹 NUEVO
        public decimal AporteDiarioSugerido { get; set; }    // 🔹 opcional, por si lo necesitás en el futuro

        public decimal PorcentajeAvance { get; set; }
    }
}

