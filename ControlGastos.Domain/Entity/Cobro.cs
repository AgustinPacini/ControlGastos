using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Entity
{
    public class Cobro
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public TipoCobro Tipo { get; set; }

        public enum TipoCobro
        {
            Debito,
            Efectivo,
            Credito,
            Tranferencia
        }
    }
}
