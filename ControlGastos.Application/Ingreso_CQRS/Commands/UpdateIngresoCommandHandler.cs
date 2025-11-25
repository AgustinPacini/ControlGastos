using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Ingreso_CQRS.Commands
{
    public class UpdateIngresoCommandHandler : IRequestHandler<UpdateIngresoCommand, bool>
    {
        private readonly IBaseRepository<Ingresos> _baseRepository;

        public UpdateIngresoCommandHandler(IBaseRepository<Ingresos> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<bool> Handle(UpdateIngresoCommand request, CancellationToken cancellationToken)
        {
            var ingreso = await _baseRepository.GetById(request.Id);
            if (ingreso == null) return false;

            ingreso.Fuente = request.IngresoDto.Fuente ?? ingreso.Fuente;
            ingreso.Monto = request.IngresoDto.Monto;
            ingreso.Fecha = request.IngresoDto.Fecha;
            ingreso.MetodoRecepcion = request.IngresoDto.MetodoRecepcion ?? ingreso.MetodoRecepcion;
            ingreso.Notas = request.IngresoDto.Notas;
            ingreso.CategoriaId = request.IngresoDto.CategoriaId;

            await _baseRepository.UpdateAsync(ingreso);
            return true;
        }
    }
}
