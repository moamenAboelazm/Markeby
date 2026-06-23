namespace Library.Models
{
    public class BoatImage
    {
        public Guid Id { get; set; }
        public Guid BoatId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public Boat Boat { get; set; } = null!;
    }
}
