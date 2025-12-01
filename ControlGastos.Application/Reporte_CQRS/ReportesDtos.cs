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
    public class DashboardResumenDto
    {
        public decimal TotalIngresosMes { get; set; }
        public decimal TotalGastosMes { get; set; }
        public decimal BalanceMes { get; set; }

        public List<TopCategoriaItem> TopCategorias { get; set; } = new();
        public List<TendenciaMensualItem> Tendencias { get; set; } = new();
    }
}
