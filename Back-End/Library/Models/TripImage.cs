namespace Library.Models
{
    public class TripImage
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Trip Trip { get; set; } = null!;
    }
}
