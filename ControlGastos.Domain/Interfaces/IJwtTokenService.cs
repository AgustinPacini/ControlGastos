using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerarToken(string nombreUsuario, int usuarioId);
    }
}
