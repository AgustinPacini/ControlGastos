using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Login
{
    public record LoginCommand(LoginDto loginDto) : IRequest<LoginResponseDto>;
}
