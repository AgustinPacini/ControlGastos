using ControlGastos.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Interfaces
{
    public interface ICobroRepository
    {
        Task<IEnumerable<Cobro>> GetByMonth(int year, int month);
        decimal ObtenerTotalCobrosPorMes(int year, int month);
    }
}
