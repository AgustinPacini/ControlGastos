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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ControlGastosDbContext _db;

        public UsuarioRepository(ControlGastosDbContext db)
        {
            _db = db;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _db.Usuarios.FindAsync(id);
        }

        public async Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario)
        {
            return await _db.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario)
        {
            return await _db.Usuarios.AnyAsync(u => u.NombreUsuario == nombreUsuario);
        }

        
    }
}
