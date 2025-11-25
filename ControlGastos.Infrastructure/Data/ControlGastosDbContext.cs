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

    public DbSet<MetaAhorro>MetaAhorros { get; set; } = null!;


    // 🔹 nuevas
    public DbSet<Cuenta> Cuentas { get; set; } = null!;
    public DbSet<GastoFijo> GastosFijos { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =======================
        // GASTOS
        // =======================
        modelBuilder.Entity<Gasto>()
            .HasOne(g => g.Categoria)
            .WithMany(c => c.Gastos)
            .HasForeignKey(g => g.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Gasto -> Usuario (N:1)
        modelBuilder.Entity<Gasto>()
            .HasOne(g => g.Usuario)
            .WithMany(u => u.Gastos)
            .HasForeignKey(g => g.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // Gasto -> Cuenta (N:1 opcional)
        modelBuilder.Entity<Gasto>()
            .HasOne(g => g.Cuenta)
            .WithMany(c => c.Gastos)
            .HasForeignKey(g => g.CuentaId)
            .OnDelete(DeleteBehavior.SetNull);

        // =======================
        // INGRESOS
        // =======================
        modelBuilder.Entity<Ingresos>()
            .HasOne(i => i.Categoria)
            .WithMany(c => c.Ingresos)
            .HasForeignKey(i => i.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ingreso -> Usuario (N:1)
        modelBuilder.Entity<Ingresos>()
            .HasOne(i => i.Usuario)
            .WithMany(u => u.Ingresos)
            .HasForeignKey(i => i.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ingreso -> Cuenta (N:1 opcional)
        modelBuilder.Entity<Ingresos>()
            .HasOne(i => i.Cuenta)
            .WithMany(c => c.Ingresos)
            .HasForeignKey(i => i.CuentaId)
            .OnDelete(DeleteBehavior.SetNull);

        // =======================
        // PRESUPUESTOS
        // =======================
        modelBuilder.Entity<Presupuesto>()
            .HasOne(p => p.Categoria)
            .WithMany()
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Presupuesto -> Usuario (N:1)
        modelBuilder.Entity<Presupuesto>()
            .HasOne(p => p.Usuario)
            .WithMany(u => u.Presupuestos)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // =======================
        // METAS DE AHORRO
        // =======================
        modelBuilder.Entity<MetaAhorro>()
            .HasOne(m => m.Usuario)
            .WithMany(u => u.MetasAhorro)
            .HasForeignKey(m => m.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // =======================
        // CUENTAS
        // =======================
        modelBuilder.Entity<Cuenta>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.Cuentas)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // =======================
        // GASTOS FIJOS
        // =======================
        modelBuilder.Entity<GastoFijo>()
            .HasOne(gf => gf.Usuario)
            .WithMany(u => u.GastosFijos)
            .HasForeignKey(gf => gf.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GastoFijo>()
            .HasOne(gf => gf.Categoria)
            .WithMany()
            .HasForeignKey(gf => gf.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GastoFijo>()
            .HasOne(gf => gf.Cuenta)
            .WithMany(c => c.GastosFijos)
            .HasForeignKey(gf => gf.CuentaId)
            .OnDelete(DeleteBehavior.SetNull);

        // =======================
        // CATEGORÍAS
        // =======================
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.Property(c => c.Nombre)
                  .IsRequired();

            entity.Property(c => c.TipoCategoria)
                  .IsRequired()
                  .HasMaxLength(20)
                  .HasDefaultValue("Ambos");
        });
    }
}
