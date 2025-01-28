using ControlGastos.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario);
        Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario);
       
        
    }
}
