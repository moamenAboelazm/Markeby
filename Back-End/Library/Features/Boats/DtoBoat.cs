using Library.Enums;

namespace Library.Features.Boats
{
    public class DtoBoats
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? MainImageUrl { get; set; }
        public bool HasWifi { get; set; }
        public bool HasFoodFacility { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class DtoBoat
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? MainImageUrl { get; set; }
        public int YearBuilt { get; set; }
        public double MaxSpeed { get; set; }
        public bool HasWifi { get; set; }
        public bool HasFoodFacility { get; set; }
        public string Status { get; set; } = string.Empty;
        public IReadOnlyList<DtoBoatImage> Images { get; set; } = new List<DtoBoatImage>();
        public IReadOnlyList<DtoBoatTripsTable> Trips { get; set; } = new List<DtoBoatTripsTable>();

    }

    public class DtoBoatTripsTable
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CaptainName { get; set; }
        public string CaptainImg { get; set; }
        public string StartLocation { get; set; } = string.Empty;
        public TripType Type { get; set; }
        public TripStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class DtoBoatImage
    {
        public string ImageUrl { get; set; } = string.Empty;
    }


    public class BoatDashboardStatsDto
    {
        public int TotalBoats { get; set; }
        public int AtSea { get; set; }
        public int Available { get; set; }
        public int OutOfService { get; set; }
    }
}
