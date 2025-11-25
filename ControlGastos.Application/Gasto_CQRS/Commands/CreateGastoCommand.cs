using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Commands
{
    public record CreateGastoCommand(int UsuarioId, GastoDto GastoDto) : IRequest<int>;
    public record UpdateGastoCommand(int Id, int UsuarioId, GastoDto GastoDto) : IRequest<bool>;

}
