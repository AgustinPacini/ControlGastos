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
    public DbSet<Categoria> Categorias { get; set; } = null!;
    public DbSet<Presupuesto> Presupuestos { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de relaciones y restricciones
        modelBuilder.Entity<Gasto>()
            .HasOne(g => g.Categoria)
            .WithMany(c => c.Gastos)
            .HasForeignKey(g => g.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ingresos>()
            .HasOne(i => i.Categoria)
            .WithMany(c => c.Ingresos)
            .HasForeignKey(i => i.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Presupuesto>()
                .HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
