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
        private readonly IGastoRepository _gastoRepository;

        public GetAllGastosQueryHandler(IGastoRepository gastoRepository)
        {
            _gastoRepository = gastoRepository;
        }

        /// <summary>
        /// Obtiene todos los gastos desde el repositorio.
        /// </summary>
        public async Task<List<Gasto>> Handle(GetAllGastosQuery request, CancellationToken cancellationToken)
        {
            var gastos = await _gastoRepository.GetByUsuarioAsync(request.UsuarioId, cancellationToken);
            return gastos;
        }
    }
}
