using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Reporte.Queries.GastosPorCategoria
{

    public class GastoPorCategoriaResult
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal TotalGastado { get; set; }
    }
}
