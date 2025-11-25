using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Categoria_CQRS.Commands
{
    public class UpdateCategoriaCommandHandler : IRequestHandler<UpdateCategoriaCommand, bool>
    {
        private readonly IBaseRepository<CategoriaDto> _baseRepository;

        public UpdateCategoriaCommandHandler(IBaseRepository<CategoriaDto> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<bool> Handle(UpdateCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _baseRepository.GetById(request.Id);
            if (categoria is null) return false;

            categoria.Nombre = request.Nombre;
            categoria.Descripcion = request.Descripcion;
            categoria.TipoCategoria = request.TipoCategoria;

            await _baseRepository.UpdateAsync(categoria);
            return true;
        }
    }
}
