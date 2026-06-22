using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Library.Features.Boats.Commands
{
    public class UpdateBoatCommand : IRequest<bool>
    {
        [JsonIgnore]
        [BindNever]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int YearBuilt { get; set; }
        public bool HasWifi { get; set; }
        public bool HasFoodFacility { get; set; }
        public bool HasToilet { get; set; }
        public BoatStatus Status { get; set; }
        public List<IFormFile>? Images { get; set; }
    }

    public class UpdateBoatCommandHandler : IRequestHandler<UpdateBoatCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IMemoryCache _cache;

        public UpdateBoatCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _cache = cache;
        }

        public async Task<bool> Handle(UpdateBoatCommand data, CancellationToken cancellationToken)
        {
            var boat = await _unitOfWork.Boats.GetBoatWithDetailsAsync(data.Id);

            if (boat == null) return false;

            _mapper.Map(data, boat);

            boat.Captains.Clear();

            /*
                         if (data.CaptainIds != null && data.CaptainIds.Any())
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

            if (data.Images != null && data.Images.Any())
            {
                foreach (var oldImage in boat.Images)
                {
                    _fileService.DeleteFile(oldImage.ImageUrl);
                }

                boat.Images.Clear();

                foreach (var file in data.Images)
                {
                    var imageUrl = await _fileService.SaveFileAsync(file, "boats");
                    boat.Images.Add(new BoatImage { ImageUrl = imageUrl });
                }
            }

            _unitOfWork.Boats.Update(boat);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllBoatsCacheKey");
            _cache.Remove("ActiveBoatsCacheKey");
            _cache.Remove($"BoatDetailsCacheKey_{data.Id}");

            return true;
        }
    }
}