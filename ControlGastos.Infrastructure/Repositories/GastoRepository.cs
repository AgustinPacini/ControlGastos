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
    public class GastoRepository : IGastoRepository
    {
        private readonly ControlGastosDbContext _context;

        public GastoRepository(ControlGastosDbContext context)
        {
            _context = context;
        }

        
        public async Task<IEnumerable<Gasto>> GetByMonth(int year, int month)
        {
            return await _context.Gastos.Where(c => c.Fecha.Year == year && c.Fecha.Month == month).ToListAsync();
        }
        public decimal ObtenerTotalGastosPorMes(int year, int month)
        {
            return _context.Set<Gasto>()
                .Where(g => g.Fecha.Month == month && g.Fecha.Year == year)
                .Sum(g => g.Monto);
        }

        

       
    }
}
