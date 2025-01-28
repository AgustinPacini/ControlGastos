using Microsoft.EntityFrameworkCore;
using ControlGastos.Domain.Entity;
namespace ControlGastos.Infrastructure.Data;

public class ControlGastosDbContext : DbContext
{
    public ControlGastosDbContext(DbContextOptions<ControlGastosDbContext> options)
        : base(options)
    {
    }

    public DbSet<Gasto> Gastos { get; set; } = null!;
    public DbSet<Ingresos> Ingresos { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
}
