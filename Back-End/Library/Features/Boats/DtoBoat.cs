namespace Library.Features.Boats
{
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
    }
    public class BoatDashboardStatsDto
    {
        public int TotalBoats { get; set; }
        public int AtSea { get; set; }
        public int Available { get; set; }
        public int OutOfService { get; set; }
    }

    public class BoatListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CaptainName { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
