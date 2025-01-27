

namespace ControlGastos.Domain.Entity
{
    public class Cobro
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public TipoCobro Tipo { get; set; }

        public enum TipoCobro
        {
            Debito,
            Efectivo,
            Credito,
            Tranferencia
        }
    }
}
