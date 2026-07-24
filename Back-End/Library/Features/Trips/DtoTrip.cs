
using Library.Models;

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

    public class DtoTripforUser
    {
        public Guid Id { get; set; }
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
        public string? Img { get; set; }
    }

    public class DtoTripById
    {
        public Guid Id { get; set; }
        public Guid BoatId { get; set; }
        public string BoatName { get; set; }
        public Guid CaptainId { get; set; }
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
        public List<string>? ImagesURLs { get; set; }
        public string? BoatImg {  get; set; }
        public string? CaptainImg { get;set; }
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
