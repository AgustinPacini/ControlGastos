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
    public class ExportIngresosCsvQueryHandler
         : IRequestHandler<ExportIngresosCsvQuery, byte[]>
    {
        private readonly IBaseRepository<Ingresos> _ingresoRepo;

        public ExportIngresosCsvQueryHandler(IBaseRepository<Ingresos> ingresoRepo)
        {
            _ingresoRepo = ingresoRepo;
        }

        public async Task<byte[]> Handle(ExportIngresosCsvQuery request, CancellationToken cancellationToken)
        {
            var ingresos = await _ingresoRepo.GetAllAsync();

            var query = ingresos.Where(i => i.UsuarioId == request.UsuarioId);

            if (request.Desde.HasValue)
                query = query.Where(i => i.Fecha.Date >= request.Desde.Value.Date);

            if (request.Hasta.HasValue)
                query = query.Where(i => i.Fecha.Date <= request.Hasta.Value.Date);

            var lista = query
                .OrderBy(i => i.Fecha)
                .ToList();

            var sb = new StringBuilder();

            // Cabecera
            sb.AppendLine("Fecha;Monto;CategoriaId;Fuente;MetodoRecepcion;Notas");

            foreach (var i in lista)
            {
                var linea = string.Join(";", new[]
                {
                    i.Fecha.ToString("yyyy-MM-dd"),
                    i.Monto.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    i.CategoriaId.ToString(),
                    EscapeCsv(i.Fuente),
                    EscapeCsv(i.MetodoRecepcion),
                    EscapeCsv(i.Notas ?? string.Empty)
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
