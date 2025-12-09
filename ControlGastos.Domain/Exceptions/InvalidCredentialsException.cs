using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Exceptions
{
    /// <summary>
    /// Se lanza cuando las credenciales de login son inválidas.
    /// </summary>
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string? message = null)
            : base(message ?? "Usuario o contraseña incorrectos.")
        {
        }
    }
}
