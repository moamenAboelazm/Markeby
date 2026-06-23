using Library.Enums;

namespace Library.Models
{
    public class Boat
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int YearBuilt { get; set; }
        public double MaxSpeed { get; set; }
        public bool HasWifi { get; set; }
        public bool HasFoodFacility { get; set; }
        public BoatStatus Status { get; set; }

        public ICollection<BoatImage> Images { get; set; } = new List<BoatImage>();
        public ICollection<Captain> Captains { get; set; } = new List<Captain>();
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
