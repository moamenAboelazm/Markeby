using AutoMapper;
using Library.IRepository;
using Library.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Captains.Commands
{
    public class CreateCaptainCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ExperienceInfo { get; set; } = string.Empty;
    }

    public class CreateCaptainCommandHandler : IRequestHandler<CreateCaptainCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public CreateCaptainCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Guid> Handle(CreateCaptainCommand request, CancellationToken cancellationToken)
        {
            var captain = _mapper.Map<Captain>(request);

            await _unitOfWork.Captains.AddAsync(captain);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllCaptainsCacheKey");

            return captain.Id;
        }
    }
}
