using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Commands
{
    public class CreateGastoDto
    {
        public string? Concepto { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string? MetodoPago {  get; set; }
        public string? Notas { get; set; }

    }
}
