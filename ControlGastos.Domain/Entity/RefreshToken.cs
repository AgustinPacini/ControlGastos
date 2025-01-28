using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Entity
{
    public class RefreshToken
    {
        public int Id { get; set; }

        /// <summary>
        /// Usuario al que pertenece este Refresh Token
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Token opaco (p. ej. un GUID con algo de entropía adicional).
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de expiración del Refresh Token
        /// </summary>
        public DateTime Expira { get; set; }

        /// <summary>
        /// Indica si ya fue usado o revocado
        /// </summary>
        public bool Revocado { get; set; }

        // Relación con Usuario
        public Usuario? Usuario { get; set; }
    }
}
