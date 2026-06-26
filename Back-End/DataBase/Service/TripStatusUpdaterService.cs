using Library.Enums;
using Library.IRepository;
using Library.Models;
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
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        var trips = await unitOfWork.Trips.GetAllTripsWithDetailsAsync();

                        bool hasChanges = false;
                        var currentTime = DateTime.UtcNow;
                        currentTime = currentTime.AddHours(3);

                        foreach (var trip in trips)
                        {
                            if (trip.Status == TripStatus.Scheduled && trip.StartTime <= currentTime && trip.EndTime > currentTime)
                            {
                                trip.Status = TripStatus.Ongoing;

                                if (trip.Boat != null)
                                {
                                    trip.Boat.Status = BoatStatus.AtSea;
                                    unitOfWork.Boats.Update(trip.Boat);
                                }
                                    
                                hasChanges = true;
                            }
                            else if (trip.Status == TripStatus.Ongoing && trip.EndTime <= currentTime)
                            {
                                trip.Status = TripStatus.Completed;

                                if (trip.Boat != null)
                                {
                                    trip.Boat.Status = BoatStatus.Available;
                                    unitOfWork.Boats.Update(trip.Boat);
                                }

                                hasChanges = true;
                            }
                        }

                        if (hasChanges)
                        {
                            await unitOfWork.CompleteAsync();
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