namespace ControlGastos.Domain.Entity;

public class Gasto
{
    public int Id { get; set; }
    public decimal Monto { get; set; }
    public string Descripcion { get; set; }
    public DateTime Fecha { get; set; }
    public string MetodoPago { get; set; }
    public string Notas { get; set; }

    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
    // 🔹 Multiusuario
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    // 🔹 Relación con Cuenta (la agregamos para más adelante)
    public int? CuentaId { get; set; }
    public Cuenta? Cuenta { get; set; }

    public bool EsGastoFijo { get; set; } = false;
    public int? GastoFijoId { get; set; }
    public GastoFijo? GastoFijo { get; set; }


}