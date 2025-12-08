using ControlGastos.Application.Ingreso_CQRS.Commands;
using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ControlGastos.Application.Ingreso_CQRS.Queries
{
    /// <summary>
    /// Handler para obtener el listado completo de ingresos.
    /// </summary>
    public class GetAllIngresosQueryHandler : IRequestHandler<GetAllIngresosQuery, List<IngresosDto>>
    {
        private readonly IBaseRepository<Ingresos> _baseRepository;

        public GetAllIngresosQueryHandler(IBaseRepository<Ingresos> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        /// <summary>
        /// Obtiene todos los ingresos desde el repositorio y los mapea a DTOs.
        /// </summary>
        public async Task<List<IngresosDto>> Handle(GetAllIngresosQuery request, CancellationToken cancellationToken)
        {
            var ingresos = await _baseRepository.GetAllAsync();

            return ingresos
                .Where(i => i.UsuarioId == request.UsuarioId)
                .Select(i => new IngresosDto
                {
                    Id = i.Id,                                   // <- agregar
                    Fuente = i.Fuente,
                    Monto = i.Monto,
                    Fecha = i.Fecha,
                    MetodoRecepcion = i.MetodoRecepcion,
                    Notas = i.Notas,
                    CategoriaId = i.CategoriaId
                })
                .ToList();
        }
    }
}
