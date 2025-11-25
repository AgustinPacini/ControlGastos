using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Reporte_CQRS
{
    public class BalanceHistoricoItem
    {
        public int Mes { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal Balance => TotalIngresos - TotalGastos;
    }

    public class TopCategoriaItem
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal TotalGasto { get; set; }
    }

    public class TendenciaMensualItem
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal Balance => TotalIngresos - TotalGastos;
    }
}
