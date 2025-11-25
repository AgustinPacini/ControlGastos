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
    public record ExportIngresosCsvQuery(
        int UsuarioId,
        DateTime? Desde,
        DateTime? Hasta) : IRequest<byte[]>;


}
