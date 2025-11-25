using ControlGastos.Application.Export_CQRS.ControlGastos.Application.Export_CQRS;
using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Export_CQRS.Queries
{
    public class ExportGastosCsvQueryHandler
            : IRequestHandler<ExportGastosCsvQuery, byte[]>
    {
        private readonly IBaseRepository<Gasto> _gastoRepo;

        public ExportGastosCsvQueryHandler(IBaseRepository<Gasto> gastoRepo)
        {
            _gastoRepo = gastoRepo;
        }

        public async Task<byte[]> Handle(ExportGastosCsvQuery request, CancellationToken cancellationToken)
        {
            var gastos = await _gastoRepo.GetAllAsync();

            var query = gastos.Where(g => g.UsuarioId == request.UsuarioId);

            if (request.Desde.HasValue)
                query = query.Where(g => g.Fecha.Date >= request.Desde.Value.Date);

            if (request.Hasta.HasValue)
                query = query.Where(g => g.Fecha.Date <= request.Hasta.Value.Date);

            var lista = query
                .OrderBy(g => g.Fecha)
                .ToList();

            var sb = new StringBuilder();

            // Cabecera
            sb.AppendLine("Fecha;Monto;CategoriaId;Descripcion;MetodoPago;Notas");

            foreach (var g in lista)
            {
                var linea = string.Join(";", new[]
                {
                    g.Fecha.ToString("yyyy-MM-dd"),
                    g.Monto.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    g.CategoriaId.ToString(),
                    EscapeCsv(g.Descripcion),
                    EscapeCsv(g.MetodoPago),
                    EscapeCsv(g.Notas ?? string.Empty)
                });

                sb.AppendLine(linea);
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private static string EscapeCsv(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            if (input.Contains(';') || input.Contains('"') || input.Contains('\n'))
            {
                input = input.Replace("\"", "\"\"");
                return $"\"{input}\"";
            }

            return input;
        }
    }
}
