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
    public DbSet<Cobro> Cobro { get; set; }=null!;
}
