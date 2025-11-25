using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.MetasAhorro_CQRS.Command
{

    public record CreateMetaAhorroCommand(int UsuarioId, MetaAhorroDto Meta) : IRequest<int>;

    
    
}
