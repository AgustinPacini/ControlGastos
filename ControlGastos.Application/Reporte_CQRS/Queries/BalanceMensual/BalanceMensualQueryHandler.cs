using ControlGastos.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ControlGastos.Application.Reporte.Queries.BalanceMensual
{
    /// <summary>
    /// Handler que calcula el balance mensual (ingresos - gastos) para un mes/año determinados.
    /// Utiliza repositorios especializados que ejecutan los cálculos directamente en la base de datos.
    /// </summary>
    public class BalanceMensualQueryHandler : IRequestHandler<BalanceMensualQuery, BalanceMensualResult>
    {
        private readonly IGastoRepository _gastoRepository;
        private readonly IIngresoRepository _ingresoRepository;

        public BalanceMensualQueryHandler(IGastoRepository gastoRepository, IIngresoRepository ingresoRepository)
        {
            _gastoRepository = gastoRepository;
            _ingresoRepository = ingresoRepository;
        }

        public Task<BalanceMensualResult> Handle(BalanceMensualQuery request, CancellationToken cancellationToken)
        {
            // Calculamos los totales usando los métodos especializados del repositorio.
            // Esto evita traer todos los datos a memoria y hace el código más escalable.
            var totalGastos = _gastoRepository.ObtenerTotalGastosPorMes(request.Anio, request.Mes);
            var totalIngresos = _ingresoRepository.ObtenerTotalIngresoPorMes(request.Anio, request.Mes);

            var result = new BalanceMensualResult
            {
                mes = request.Mes,
                TotalIngresos = totalIngresos,
                TotalGastos = totalGastos,
                Balance = totalIngresos - totalGastos
            };

            return Task.FromResult(result);
        }
    }
}
