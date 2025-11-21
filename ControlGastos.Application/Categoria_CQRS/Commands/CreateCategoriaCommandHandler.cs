using ControlGastos.Domain.Entity;
using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Categoria_CQRS.Commands
{
    public class CreateCategoriaCommandHandler
       : IRequestHandler<CreateCategoriaCommand, int>
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CreateCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<int> Handle(
            CreateCategoriaCommand request,
            CancellationToken cancellationToken)
        {
            var categoria = new Categoria
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion
            };

            await _categoriaRepository.AddAsync(categoria);
            return categoria.Id;
        }
    }
}
