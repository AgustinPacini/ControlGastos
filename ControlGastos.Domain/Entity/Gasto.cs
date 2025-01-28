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


}