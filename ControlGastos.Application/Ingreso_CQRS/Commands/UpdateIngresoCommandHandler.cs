using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Exceptions;
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
            var ingreso = await _baseRepository.GetByIdAsync(request.Id, cancellationToken);
            if (ingreso == null)
                return false; // 404 en el controller

            // Ownership: si no es del usuario → 403
            if (ingreso.UsuarioId != request.UsuarioId)
                throw new ForbiddenAccessException("El ingreso no pertenece al usuario autenticado.");

            // Actualizar campos
            ingreso.Fuente = string.IsNullOrWhiteSpace(request.Ingresos.Fuente)
                ? ingreso.Fuente
                : request.Ingresos.Fuente;

            ingreso.Monto = request.Ingresos.Monto;
            ingreso.Fecha = request.Ingresos.Fecha;

            ingreso.MetodoRecepcion = string.IsNullOrWhiteSpace(request.Ingresos.MetodoRecepcion)
                ? ingreso.MetodoRecepcion
                : request.Ingresos.MetodoRecepcion;

            ingreso.Notas = request.Ingresos.Notas;
            ingreso.CategoriaId = request.Ingresos.CategoriaId;
            // UsuarioId no se toca (ya validado)

            await _baseRepository.UpdateAsync(ingreso, cancellationToken);
            return true;
        }
    }
}
