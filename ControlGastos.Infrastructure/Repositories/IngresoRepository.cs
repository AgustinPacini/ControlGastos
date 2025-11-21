using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using ControlGastos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlGastos.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio específico para operaciones de lectura sobre ingresos.
    /// </summary>
    public class IngresoRepository : IIngresoRepository
    {
        private readonly ControlGastosDbContext _context;

        public IngresoRepository(ControlGastosDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los ingresos correspondientes a un mes y año determinado.
        /// </summary>
        public async Task<IEnumerable<Ingresos>> GetByMonth(int year, int month)
        {
            return await _context.Ingresos
                .Where(c => c.Fecha.Year == year && c.Fecha.Month == month)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene el total de ingresos de un mes/año, calculado directamente en la base de datos.
        /// </summary>
        public decimal ObtenerTotalIngresoPorMes(int year, int month)
        {
            return _context.Set<Ingresos>()
                .Where(g => g.Fecha.Month == month && g.Fecha.Year == year)
                .Sum(g => g.Monto);
        }
    }
}
