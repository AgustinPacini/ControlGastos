using ControlGastos.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Interfaces
{
    public interface IPresupuestoRepository
    {
        Task<Presupuesto?> GetByIdAsync(int id);
        Task<List<Presupuesto>> GetByMesAnioAsync(int anio, int mes);

    }
}
