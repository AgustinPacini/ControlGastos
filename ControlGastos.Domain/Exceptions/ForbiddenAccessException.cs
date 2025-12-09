using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Exceptions
{
    /// <summary>
    /// Se lanza cuando el usuario intenta acceder a un recurso que no le pertenece.
    /// </summary>
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string? message = null)
            : base(message ?? "No tenés permisos para acceder a este recurso.")
        {
        }
    }
}
