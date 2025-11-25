using ControlGastos.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Queries
{

    public record GetAllGastosQuery(int UsuarioId) : IRequest<List<Gasto>>;
}

