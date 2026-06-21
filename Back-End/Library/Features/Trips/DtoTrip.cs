using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Trips
{
    public class DtoTrip
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
        public DateTime? MeetingTime { get; set; }
        public int AvailableSeats { get; set; }
        public string? IncludedItems { get; set; }
        public string? ExcludedItems { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
