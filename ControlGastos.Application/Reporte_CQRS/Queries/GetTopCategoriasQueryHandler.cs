using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Reporte_CQRS.Queries
{
    public class GetTopCategoriasQueryHandler
         : IRequestHandler<GetTopCategoriasQuery, List<TopCategoriaItem>>
    {
        private readonly IBaseRepository<Gasto> _gastoRepo;
        private readonly IBaseRepository<Categoria> _categoriaRepo;

        public GetTopCategoriasQueryHandler(
            IBaseRepository<Gasto> gastoRepo,
            IBaseRepository<Categoria> categoriaRepo)
        {
            _gastoRepo = gastoRepo;
            _categoriaRepo = categoriaRepo;
        }

        public async Task<List<TopCategoriaItem>> Handle(GetTopCategoriasQuery request, CancellationToken cancellationToken)
        {
            var gastos = (await _gastoRepo.GetAllAsync())
                .Where(g => g.UsuarioId == request.UsuarioId
                            && g.Fecha.Date >= request.Desde.Date
                            && g.Fecha.Date <= request.Hasta.Date)
                .ToList();

            var categorias = await _categoriaRepo.GetAllAsync();
            var categoriasDict = categorias.ToDictionary(c => c.Id, c => c.Nombre);

            var query = gastos
                .GroupBy(g => g.CategoriaId)
                .Select(g => new TopCategoriaItem
                {
                    Categoria = categoriasDict.ContainsKey(g.Key)
                        ? categoriasDict[g.Key]
                        : "Sin categoría",
                    TotalGasto = g.Sum(x => x.Monto)
                })
                .OrderByDescending(x => x.TotalGasto)
                .Take(request.TopN)
                .ToList();

            return query;
        }
    }
}
