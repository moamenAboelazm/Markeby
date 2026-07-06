using Library.Enums;

namespace Library.Features.Captains
{
    public class DtoCaptain
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public string Rank { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public IReadOnlyList<DtoCaptainTripsTable> TripsTable { get; set; } = new List<DtoCaptainTripsTable>();
    }

    public class DtoCaptainTripsTable
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string BoatName { get; set; }
        public string BoatImg { get; set; }
        public string StartLocation { get; set; } = string.Empty;
        public TripType Type { get; set; }
        public TripStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class CaptainDashboardStatsDto
    {
        public int TotalCaptains { get; set; }
        public int OnMission { get; set; }
        public int OnShoreLeave { get; set; }
    }

    public class CaptainListDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public string Vessel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? ProfilePhotoUrl { get; set; }
    }
}