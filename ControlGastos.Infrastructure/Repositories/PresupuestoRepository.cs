using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using ControlGastos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Infrastructure.Repositories
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        private readonly ControlGastosDbContext _context;

        public PresupuestoRepository(ControlGastosDbContext context)
        {
            _context = context;
        }

        public async Task<Presupuesto?> GetByIdAsync(int id)
        {
            return await _context.Presupuestos
                                 .Include(p => p.Categoria)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        
    }
}
