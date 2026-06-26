
namespace Library.Features.Trips
{
    public class DtoTrip
    {
        public Guid Id { get; set; }
        public string BoatName { get; set; }
        public string CaptainName { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StartLocation { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int BoatSeats { get; set; }
        public int AvailableSeats { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
    public class TripDashboardStatsDto
    {
        public int TotalTrips { get; set; }
        public int ScheduledTrips { get; set; }
        public int OngoingTrips { get; set; }
        public int CompletedTrips { get; set; }
        public int CancelledTrips { get; set; }
    }

    public class AvailableResourcesDto
    {
        public List<AvailableCaptainDto> Captains { get; set; } = new();
        public List<AvailableBoatDto> Boats { get; set; } = new();
    }

    public class AvailableCaptainDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }

    public class AvailableBoatDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

}
