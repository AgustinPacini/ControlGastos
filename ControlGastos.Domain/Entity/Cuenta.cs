using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Entity
{

    public class Cuenta
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;   // Ej: "Santander sueldo"
        public string Tipo { get; set; } = string.Empty;     // Ej: "Banco", "Efectivo", "Tarjeta"
        public decimal SaldoInicial { get; set; }
        public decimal SaldoActual { get; set; }

        // Multiusuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // Relación con movimientos
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
        public ICollection<Ingresos> Ingresos { get; set; } = new List<Ingresos>();
        public ICollection<GastoFijo> GastosFijos { get; set; } = new List<GastoFijo>();
    }

}
