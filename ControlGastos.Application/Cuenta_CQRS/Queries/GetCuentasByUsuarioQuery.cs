using ControlGastos.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Cuenta_CQRS.Queries
{
    public record GetCuentasByUsuarioQuery(int UsuarioId) : IRequest<List<Cuenta>>;
}
