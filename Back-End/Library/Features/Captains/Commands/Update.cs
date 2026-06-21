using AutoMapper;
using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Captains.Commands
{
    public class UpdateCaptainCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ExperienceInfo { get; set; } = string.Empty;
    }


    public class UpdateCaptainCommandHandler : IRequestHandler<UpdateCaptainCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public UpdateCaptainCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<bool> Handle(UpdateCaptainCommand request, CancellationToken cancellationToken)
        {
            var captain = await _unitOfWork.Captains.GetByIdAsync(request.Id);

            if (captain == null) return false;

            _mapper.Map(request, captain);

            _unitOfWork.Captains.Update(captain);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllCaptainsCacheKey");
            _cache.Remove($"CaptainDetailsCacheKey_{request.Id}");

            return true;
        }
    }
}
