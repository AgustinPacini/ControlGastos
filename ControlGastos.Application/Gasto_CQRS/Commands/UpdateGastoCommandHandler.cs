using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Gasto_CQRS.Commands
{

    public class UpdateGastoCommandHandler : IRequestHandler<UpdateGastoCommand, bool>
    {
        private readonly IBaseRepository<Gasto> _baseRepository;

        public UpdateGastoCommandHandler(IBaseRepository<Gasto> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<bool> Handle(UpdateGastoCommand request, CancellationToken cancellationToken)
        {
            var gasto = await _baseRepository.GetById(request.Id);
            if (gasto == null) return false;

            // 🔹 validar que el gasto sea del usuario
            if (gasto.UsuarioId != request.UsuarioId) return false;

            gasto.Descripcion = request.GastoDto.Concepto ?? gasto.Descripcion;
            gasto.Monto = request.GastoDto.Monto;
            gasto.Fecha = request.GastoDto.Fecha;
            gasto.MetodoPago = request.GastoDto.MetodoPago ?? gasto.MetodoPago;
            gasto.Notas = request.GastoDto.Notas;
            gasto.CategoriaId = request.GastoDto.CategoriaId;

            await _baseRepository.UpdateAsync(gasto);
            return true;
        }

    }
}
