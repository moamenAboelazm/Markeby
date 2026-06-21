using AutoMapper;
using Library.IRepository;
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
    public class UpdateCaptainCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public string Rank { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public IFormFile? ProfilePhoto { get; set; }
    }


    public class UpdateCaptainCommandHandler : IRequestHandler<UpdateCaptainCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IFileService _fileService;

        public UpdateCaptainCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
            _fileService = fileService;
        }

        public async Task<bool> Handle(UpdateCaptainCommand data, CancellationToken cancellationToken)
        {
            var captain = await _unitOfWork.Captains.GetByIdAsync(data.Id);

            if (captain == null) return false;

            _mapper.Map(data, captain);

            if (data.ProfilePhoto != null)
            {
                if (!string.IsNullOrEmpty(captain.ProfilePhotoUrl))
                {
                    _fileService.DeleteFile(captain.ProfilePhotoUrl);
                }

                captain.ProfilePhotoUrl = await _fileService.SaveFileAsync(data.ProfilePhoto, "captains");
            }

            _unitOfWork.Captains.Update(captain);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllCaptainsCacheKey");
            _cache.Remove($"CaptainDetailsCacheKey_{data.Id}");

            return true;
        }
    }
}
