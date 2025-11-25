using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cuenta_CQRS
{
    public class CuentaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal SaldoInicial { get; set; }
        public decimal SaldoActual { get; set; }   // podés arrancarlo igual al inicial
    }
}
