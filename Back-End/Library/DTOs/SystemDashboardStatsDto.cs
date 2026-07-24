using Library.Features.Boats;
using Library.Features.Trips;

public class SystemDashboardStatsDto
{
    public int TotalBookings { get; set; }
    public int TotalTrips { get; set; }
    public int TotalCaptains { get; set; }
    public int TotalBoats { get; set; }
    public decimal TotalEarnings { get; set; }
    public int CanceledBookings { get; set; }
    public IEnumerable<DtoBoat> Boats { get; set; } = new List<DtoBoat>();
    public IEnumerable<DtoTrip> Trips { get; set; } = new List<DtoTrip>();
}