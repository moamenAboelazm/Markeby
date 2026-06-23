using AutoMapper;
using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Features.Boats.Commands
{
    public class CreateBoatCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int YearBuilt { get; set; }
        public double MaxSpeed { get; set; }
        public bool HasWifi { get; set; }
        public bool HasFoodFacility { get; set; }
        //public List<Guid> CaptainIds { get; set; } = new List<Guid>();

        public List<IFormFile>? Images { get; set; }
    }

    public class CreateBoatCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, IFileService _fileService, IMemoryCache _cache) : IRequestHandler<CreateBoatCommand, Guid>
    {
        public async Task<Guid> Handle(CreateBoatCommand data, CancellationToken cancellationToken)
        {
            var boat = _mapper.Map<Boat>(data);
            boat.Status = BoatStatus.AtSea;

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