using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Entity
{
    public class RefreshToken
    {
     public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime Creado { get; set; }

    public string? CreadoPorIp { get; set; }

    public DateTime Expira { get; set; }

    public bool Revocado { get; set; }

    public DateTime? RevocadoEn { get; set; }

    public string? RevocadoPorIp { get; set; }

    public Usuario? Usuario { get; set; }
    }
}
