using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Presupuesto_CQRS.Queries
{
    public class PresupuestoMensualResult
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal MontoLimite { get; set; }
        public decimal MontoGastado { get; set; }
        public decimal Disponible => MontoLimite - MontoGastado;
        public decimal PorcentajeUsado =>
            MontoLimite == 0 ? 0 : Math.Round(MontoGastado / MontoLimite * 100, 2);

        public int Mes { get; set; }
        public int Anio { get; set; }

        public int? CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }
    }
}
