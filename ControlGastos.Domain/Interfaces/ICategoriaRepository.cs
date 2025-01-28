using ControlGastos.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Interfaces
{
 
   public interface ICategoriaRepository
    {
        Task<Categoria?> GetByIdAsync(int id);
      
    }
}
