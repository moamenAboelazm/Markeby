using Library.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Boats.Commands
{
    public class DeleteBoatCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteBoatCommandHandler : IRequestHandler<DeleteBoatCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBoatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteBoatCommand request, CancellationToken cancellationToken)
        {
            var boat = await _unitOfWork.Boats.GetByIdAsync(request.Id);

            if (boat == null) return false;

            _unitOfWork.Boats.Delete(boat);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
