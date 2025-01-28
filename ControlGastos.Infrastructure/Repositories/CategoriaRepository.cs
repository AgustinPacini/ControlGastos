using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using ControlGastos.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ControlGastosDbContext _context;

        public CategoriaRepository(ControlGastosDbContext context)
        {
            _context = context;
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        
    }
}
