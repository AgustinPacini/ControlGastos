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
    public class IngresoRepository : IIngresoRepository
    {
        private readonly ControlGastosDbContext _context;
        public IngresoRepository(ControlGastosDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Ingresos>> GetByMonth(int year, int month)
        {
            return await _context.Ingresos.Where(c => c.Fecha.Year == year && c.Fecha.Month == month).ToListAsync();
        }
        public decimal ObtenerTotalIngresoPorMes(int year, int month)
        {
            return _context.Set<Ingresos>()
                .Where(g => g.Fecha.Month == month && g.Fecha.Year == year)
                .Sum(g => g.Monto);
        }
    }
}
