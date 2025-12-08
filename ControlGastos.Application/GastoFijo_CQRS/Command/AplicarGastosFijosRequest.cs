using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.GastoFijo_CQRS.Command
{
    public class AplicarGastosFijosRequest
    {
        public DateTime? FechaReferencia { get; set; }
    }
}
