using AutoMapper;
using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
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
        public string Type { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public double MaxSpeed { get; set; }
        public int YearBuilt { get; set; }
        public bool HasWifi { get; set; }
        public bool HasFoodFacility { get; set; }
        public BoatStatus Status { get; set; }
        public List<IFormFile>? Images { get; set; }
    }

    public class UpdateBoatCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, IFileService _fileService, IMemoryCache _cache) : IRequestHandler<UpdateBoatCommand, bool>
    {
        public async Task<bool> Handle(UpdateBoatCommand data, CancellationToken cancellationToken)
        {
            var boat = await _unitOfWork.Boats.GetBoatForUpdateAsync(data.Id);

            if (boat == null) return false;

            _mapper.Map(data, boat);

            /*
               boat.Captains.Clear();


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
                    _fileService.DeleteFile(oldImage.ImageUrl);
                
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