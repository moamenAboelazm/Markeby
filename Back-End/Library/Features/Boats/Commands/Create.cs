using AutoMapper;
using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace Library.Features.Boats.Commands
{
    public class CreateBoatCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int YearBuilt { get; set; }
        public bool HasWifi { get; set; }
        public bool HasFoodFacility { get; set; }
        public bool HasToilet { get; set; }
        //public List<Guid> CaptainIds { get; set; } = new List<Guid>();

        public List<IFormFile>? Images { get; set; }
    }

    public class CreateBoatCommandHandler : IRequestHandler<CreateBoatCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IMemoryCache _cache;

        public CreateBoatCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _cache = cache;
        }

        public async Task<Guid> Handle(CreateBoatCommand data, CancellationToken cancellationToken)
        {
            var boat = _mapper.Map<Boat>(data);
            boat.Status = BoatStatus.Active;

            /* if (data.CaptainIds != null && data.CaptainIds.Any())
            {
                foreach (var captainId in data.CaptainIds)
                {
                    var captain = await _unitOfWork.Captains.GetByIdAsync(captainId);
                    if (captain != null)
                    {
                        boat.Captains.Add(captain);
                    }
                }
            }
             */

            boat.Images ??= new List<BoatImage>();
            if (data.Images != null && data.Images.Any())
            {
                foreach (var file in data.Images)
                {
                    var imageUrl = await _fileService.SaveFileAsync(file, "boats");
                    boat.Images.Add(new BoatImage { ImageUrl = imageUrl });
                }
            }

            await _unitOfWork.Boats.AddAsync(boat);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllBoatsCacheKey");

            return boat.Id;
        }
    }

}