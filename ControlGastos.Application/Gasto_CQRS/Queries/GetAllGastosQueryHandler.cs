using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Queries
{
    /// <summary>
    /// Handler para obtener el listado completo de gastos.
    /// </summary>
    public class GetAllGastosQueryHandler : IRequestHandler<GetAllGastosQuery, List<Gasto>>
    {
        private readonly IBaseRepository<Gasto> _baseRepository;

        public GetAllGastosQueryHandler(IBaseRepository<Gasto> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        /// <summary>
        /// Obtiene todos los gastos desde el repositorio.
        /// </summary>
        public async Task<List<Gasto>> Handle(GetAllGastosQuery request, CancellationToken cancellationToken)
        {
            var gastos = await _baseRepository.GetAllAsync();
            return gastos.ToList();
        }
    }
}
