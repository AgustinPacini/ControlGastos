using ControlGastos.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlGastos.Domain.Entity;

namespace ControlGastos.Domain.Interfaces
{
    public interface IGastoRepository
    {
        Task<IEnumerable<Gasto>> GetByMonth(int year, int month);
        decimal ObtenerTotalGastosPorMes(int year, int month);
        
        
    }
}
