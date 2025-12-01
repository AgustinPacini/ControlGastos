using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Presupuesto_CQRS.Queries
{
    public class GetPresupuestosQueryHandler
        : IRequestHandler<GetPresupuestosQuery, List<PresupuestoDto>>
    {
        private readonly IBaseRepository<Domain.Entity.Presupuesto> _presupuestoRepository;

        public GetPresupuestosQueryHandler(IBaseRepository<Domain.Entity.Presupuesto> presupuestoRepository)
        {
            _presupuestoRepository = presupuestoRepository;
        }

        public async Task<List<PresupuestoDto>> Handle(
            GetPresupuestosQuery request,
            CancellationToken cancellationToken)
        {
            var presupuestos = await _presupuestoRepository.GetAllAsync();

            return presupuestos
                .Where(p => p.UsuarioId == request.UsuarioId)
                .Select(p => new PresupuestoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    MontoLimite = p.MontoLimite,
                    Mes = p.Mes,
                    Anio = p.Anio,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria != null ? p.Categoria.Nombre : null
                })
                .ToList();
        }
    }
}
