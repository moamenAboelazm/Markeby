using AutoMapper;
using Library.IRepository;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        public string FullName { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public string Rank { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public IFormFile? ProfilePhoto { get; set; }
    }

    public class CreateCaptainCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, IMemoryCache _cache, IFileService _fileService) : IRequestHandler<CreateCaptainCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCaptainCommand data, CancellationToken cancellationToken)
        {
            var captain = _mapper.Map<Captain>(data);

            if (data.ProfilePhoto != null)
            {
                captain.ProfilePhotoUrl = await _fileService.SaveFileAsync(data.ProfilePhoto, "captains");
            }

            await _unitOfWork.Captains.AddAsync(captain);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllCaptainsCacheKey");

            return captain.Id;
        }
    }
}
