using ControlGastos.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Interfaces
{
    public interface IIngresoRepository
    {
        Task<IEnumerable<Ingresos>> GetByMonth(int year, int month);
        decimal ObtenerTotalIngresoPorMes(int year, int month);
    }
}
