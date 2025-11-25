using ControlGastos.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Interfaces
{
    public interface ICategoriaRepository 
    {
        Task<Categoria?> GetByIdAsync(int id);

        // NUEVOS
        Task<IEnumerable<Categoria>> GetAllAsync();
        Task<Categoria> AddAsync(Categoria categoria);
       
    }
}
