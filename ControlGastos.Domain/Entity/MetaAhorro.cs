using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Entity
{
    public class MetaAhorro
    {
        public int Id { get; set; }
        public string NombreObjetivo { get; set; } = string.Empty;  // "Zapatillas Nike", "Viaje a Bariloche"
        public decimal MontoObjetivo { get; set; }
        public decimal MontoAhorrado { get; set; }   // acumulado
        public DateTime FechaObjetivo { get; set; }  // fecha límite deseada
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Multiusuario (para más adelante)
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
