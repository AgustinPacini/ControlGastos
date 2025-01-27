namespace ControlGastos.Domain.Entity;

public class Gasto
{
    public int Id { get; set; }
    public decimal Monto { get; set; }
    public string? Descripcion { get; set; }
    public DateTime Fecha { get; set; }



    public TipoGasto Tipo { get; set; }

    public enum TipoGasto
    {
        Debito,
        Efectivo,
        Credito,
        Tranferencia
    }
}