using Library.Enums;
using Library.IRepository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataBase.Service
{
    public class TripStatusUpdaterService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TripStatusUpdaterService> _logger;

        public TripStatusUpdaterService(IServiceProvider serviceProvider, ILogger<TripStatusUpdaterService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        var trips = await unitOfWork.Trips.GetAllAsync();

                        bool hasChanges = false;

                        foreach (var trip in trips)
                        {
                            if (trip.Status == TripStatus.Scheduled && trip.StartTime <= DateTime.UtcNow)
                            {
                                trip.Status = TripStatus.Ongoing;
                                unitOfWork.Trips.Update(trip);
                                hasChanges = true;
                            }
                            else if (trip.Status == TripStatus.Ongoing && trip.EndTime <= DateTime.UtcNow)
                            {
                                trip.Status = TripStatus.Completed;
                                unitOfWork.Trips.Update(trip);
                                hasChanges = true;
                            }
                        }

                        if (hasChanges)
                        {
                            await unitOfWork.CompleteAsync();
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
