using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.GastoFijo_CQRS
{
    public class GastoFijoDto
    {
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime FechaInicio { get; set; }
        public string Periodicidad { get; set; } = "Mensual";
        public int DiaReferencia { get; set; } = 1;
        public bool Activo { get; set; } = true;

        public int? CategoriaId { get; set; }
        public int? CuentaId { get; set; }
    }
}
