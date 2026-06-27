using Library.Enums;
using Library.IRepository;
using Library.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataBase.Service
{
    public class TripStatusUpdaterService(IServiceProvider _serviceProvider, ILogger<TripStatusUpdaterService> _logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var _cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();

                        var trips = await _unitOfWork.Trips.GetAllTripsWithDetailsAsync();

                        bool hasChanges = false;
                        var currentTime = DateTime.UtcNow;
                        currentTime = currentTime.AddHours(3);

                        foreach (var trip in trips)
                        {
                            bool changed = false;
                            if (trip.Status == TripStatus.Scheduled && trip.StartTime <= currentTime && trip.EndTime > currentTime)
                            {
                                trip.Status = TripStatus.Ongoing;
                                _unitOfWork.Trips.Update(trip);

                                if (trip.Boat != null)
                                {
                                    trip.Boat.Status = BoatStatus.AtSea;
                                    _unitOfWork.Boats.Update(trip.Boat);
                                }

                                changed = true;
                            }
                            else if ( (trip.Status == TripStatus.Ongoing || trip.Status == TripStatus.Scheduled) && trip.EndTime <= currentTime)
                            {
                                trip.Status = TripStatus.Completed;
                                _unitOfWork.Trips.Update(trip);

                                if (trip.Boat != null)
                                {
                                    trip.Boat.Status = BoatStatus.Available;
                                    _unitOfWork.Boats.Update(trip.Boat);
                                }

                                changed = true;
                            }
                            hasChanges |= changed;
                            if (changed)
                            {
                                _cache.Remove($"BoatDetailsCacheKey_{trip.BoatId}");
                                _cache.Remove($"TripDetailsCacheKey_{trip.Id}");
                            }
                        }

                        if (hasChanges)
                        {
                            await _unitOfWork.CompleteAsync();
                            _cache.Remove("AllTripsCacheKey");
                            _cache.Remove("AllCaptainsCacheKey");
                            _cache.Remove("AllBoatsCacheKey");
                            _logger.LogInformation("Trip statuses updated successfully.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing TripStatusUpdaterService.");
                }

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}