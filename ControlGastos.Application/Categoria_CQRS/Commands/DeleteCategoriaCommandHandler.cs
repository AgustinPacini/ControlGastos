using ControlGastos.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGastos.Application.Categoria_CQRS.Commands
{
    public class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommand, bool>
    {
        private readonly IBaseRepository<CategoriaDto> _baseRepository;

        public DeleteCategoriaCommandHandler(IBaseRepository<CategoriaDto> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<bool> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _baseRepository.GetById(request.Id);
            if (categoria is null) return false;

            await _baseRepository.DeleteAsync(categoria,cancellationToken);
            return true;
        }
    }
}
