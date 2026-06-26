using Library.Enums;

namespace Library.Models
{
    public class Trip
    {
        public Guid Id { get; set; }
        public Guid BoatId { get; set; }
        public Guid CaptainId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StartLocation { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int AvailableSeats { get; set; }
        public TripType Type { get; set; }
        public TripStatus Status { get; set; }

        public Boat Boat { get; set; } = null!;
        public Captain Captain { get; set; } = null!;
        public ICollection<TripImage> Images { get; set; } = new List<TripImage>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<AppUser> Passengers { get; set; } = new List<AppUser>();
    }
}