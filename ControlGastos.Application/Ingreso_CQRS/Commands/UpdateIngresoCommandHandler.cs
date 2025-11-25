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

            ingreso.Fuente = request.Ingresos.Fuente ?? ingreso.Fuente;
            ingreso.Monto = request.Ingresos.Monto;
            ingreso.Fecha = request.Ingresos.Fecha;
            ingreso.MetodoRecepcion = request.Ingresos.MetodoRecepcion ?? ingreso.MetodoRecepcion;
            ingreso.Notas = request.Ingresos.Notas;
            ingreso.CategoriaId = request.Ingresos.CategoriaId;
            ingreso.UsuarioId = request.UsuarioId;

            await _baseRepository.UpdateAsync(ingreso);
            return true;
        }
    }
}
