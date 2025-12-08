using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.GastoFijo_CQRS.Command
{
    public class AplicarGastosFijosDelMesCommandHandler
       : IRequestHandler<AplicarGastosFijosDelMesCommand, int>
    {
        private readonly IBaseRepository<GastoFijo> _gastoFijoRepo;
        private readonly IBaseRepository<Gasto> _gastoRepo;
        private readonly IBaseRepository<Cuenta> _cuentaRepo;

        public AplicarGastosFijosDelMesCommandHandler(
            IBaseRepository<GastoFijo> gastoFijoRepo,
            IBaseRepository<Gasto> gastoRepo,
            IBaseRepository<Cuenta> cuentaRepo)
        {
            _gastoFijoRepo = gastoFijoRepo;
            _gastoRepo = gastoRepo;
            _cuentaRepo = cuentaRepo;
        }

        public async Task<int> Handle(AplicarGastosFijosDelMesCommand request, CancellationToken cancellationToken)
        {
            var referencia = request.FechaReferencia.Date;
            var year = referencia.Year;
            var month = referencia.Month;

            // 1) Traigo TODOS los gastos fijos y filtro por usuario/activo/fecha
            var todosGastosFijos = await _gastoFijoRepo.GetAllAsync();

            var gastosFijos = todosGastosFijos
                .Where(gf =>
                    gf.UsuarioId == request.UsuarioId &&
                    gf.Activo &&
                    gf.FechaInicio.Date <= referencia &&
                    string.Equals(gf.Periodicidad, "Mensual", StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

            if (!gastosFijos.Any())
                return 0;

            // 2) Traigo los gastos del usuario para controlar duplicados
            var todosGastos = await _gastoRepo.GetAllAsync();

            var gastosExistentes = todosGastos
                .Where(g =>
                    g.UsuarioId == request.UsuarioId &&
                    g.EsGastoFijo &&
                    g.GastoFijoId.HasValue
                )
                .ToList();

            int generados = 0;

            foreach (var gf in gastosFijos)
            {
                // Día efectivo del vencimiento (por si el mes tiene menos días)
                int dia = Math.Min(
                    gf.DiaReferencia,
                    DateTime.DaysInMonth(year, month));

                var fechaVencimiento = new DateTime(year, month, dia);

                // No aplicar si aún no llegó la fecha de vencimiento
                if (fechaVencimiento > referencia)
                    continue;

                // No aplicar si la fecha de vencimiento es anterior a la fecha de inicio del gasto fijo
                if (fechaVencimiento < gf.FechaInicio.Date)
                    continue;

                // ¿Ya existe un gasto generado para este gasto fijo en este mes?
                bool yaGenerado = gastosExistentes.Any(g =>
                    g.GastoFijoId == gf.Id &&
                    g.Fecha.Year == year &&
                    g.Fecha.Month == month &&
                    g.Fecha.Day == fechaVencimiento.Day);

                if (yaGenerado)
                    continue;

                if (!gf.CategoriaId.HasValue)
                    throw new InvalidOperationException(
                        $"El gasto fijo '{gf.Descripcion}' debe tener una categoría para poder aplicarse.");

                // 3) Creo el gasto real
                var gasto = new Gasto
                {
                    Descripcion = gf.Descripcion,
                    Monto = gf.Monto,
                    Fecha = fechaVencimiento,
                    CategoriaId = gf.CategoriaId.Value,
                    CuentaId = gf.CuentaId,
                    UsuarioId = gf.UsuarioId,
                    MetodoPago = "Gasto fijo automático",
                    Notas = $"Generado automáticamente desde GastoFijo Id={gf.Id}",
                    EsGastoFijo = true,
                    GastoFijoId = gf.Id
                };

                await _gastoRepo.AddAsync(gasto);
                generados++;

                // 4) Si tiene cuenta asociada, descuento el saldo
                if (gf.CuentaId.HasValue)
                {
                    var cuenta = await _cuentaRepo.GetById(gf.CuentaId.Value);

                    if (cuenta.UsuarioId != gf.UsuarioId)
                        throw new InvalidOperationException("La cuenta asociada al gasto fijo no pertenece al usuario.");

                    cuenta.SaldoActual -= gf.Monto;
                    await _cuentaRepo.UpdateAsync(cuenta);
                }
            }

            return generados;
        }
    }
}
