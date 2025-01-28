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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ControlGastosDbContext _db;

        public RefreshTokenRepository(ControlGastosDbContext db)
        {
            _db = db;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _db.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _db.RefreshTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            _db.RefreshTokens.Update(refreshToken);
            await _db.SaveChangesAsync();
        }
    }
}
