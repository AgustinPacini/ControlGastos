using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ControlGastos.Application.Ingreso_CQRS.Commands
{
    /// <summary>
    /// Handler encargado de crear un nuevo ingreso en el sistema.
    /// </summary>
    public class CreateIngresoCommandHandler : IRequestHandler<CreateIngresoCommand, int>
    {
        private readonly IBaseRepository<Ingresos> _baseRepository;

        public CreateIngresoCommandHandler(IBaseRepository<Ingresos> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        /// <summary>
        /// Ejecuta la creación del ingreso a partir del DTO recibido en el comando.
        /// </summary>
        /// <param name="request">Comando que contiene los datos del ingreso a crear.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Id del ingreso creado.</returns>
        public async Task<int> Handle(CreateIngresoCommand request, CancellationToken cancellationToken)
        {
            var ingresos = new Ingresos
            {
                Fuente = request.Ingresos.Fuente,
                Monto = request.Ingresos.Monto,
                Fecha = request.Ingresos.Fecha,
                MetodoRecepcion = request.Ingresos.MetodoRecepcion,
                Notas = request.Ingresos.Notas,
                CategoriaId = request.Ingresos.CategoriaId,
                UsuarioId = request.UsuarioId // 🔹
            };

            await _baseRepository.AddAsync(ingresos);
            return ingresos.Id;
        }
    }
}
