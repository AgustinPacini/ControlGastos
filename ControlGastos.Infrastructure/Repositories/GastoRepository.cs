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
    /// Repositorio específico para operaciones de lectura sobre gastos.
    /// </summary>
    public class GastoRepository : IGastoRepository
    {
        private readonly ControlGastosDbContext _context;

        public GastoRepository(ControlGastosDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los gastos correspondientes a un mes y año determinado.
        /// Incluye la navegación hacia la categoría para poder agrupar/filtrar por nombre de categoría.
        /// </summary>
        public async Task<IEnumerable<Gasto>> GetByMonth(int year, int month)
        {
            return await _context.Gastos
                .Include(g => g.Categoria)
                .Where(g => g.Fecha.Year == year && g.Fecha.Month == month)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene el total gastado en un mes/año, calculado directamente en la base de datos.
        /// </summary>
        public decimal ObtenerTotalGastosPorMes(int year, int month)
        {
            return _context.Set<Gasto>()
                .Where(g => g.Fecha.Month == month && g.Fecha.Year == year)
                .Sum(g => g.Monto);
        }
        public async Task<List<Gasto>> GetByUsuarioAsync(int usuarioId, CancellationToken cancellationToken = default)
        {
            return await _context.Gastos
                .AsNoTracking()
                .Where(g => g.UsuarioId == usuarioId)
                .ToListAsync(cancellationToken);
        }
    }
}
