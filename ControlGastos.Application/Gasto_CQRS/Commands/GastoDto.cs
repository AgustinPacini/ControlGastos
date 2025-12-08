using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Commands
{
    public class GastoDto
    {
        public string? Concepto { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string? MetodoPago { get; set; }
        public string? Notas { get; set; }

        public int CategoriaId { get; set; }

        public int? CuentaId { get; set; }
    }
}
