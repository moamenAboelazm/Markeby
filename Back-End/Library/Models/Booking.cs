using Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Booking
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public Guid TripId { get; set; }

        public int NumberOfTickets { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow.AddHours(3);

        public string? SpecialRequests { get; set; }
        public string? CancellationReason { get; set; }

        public AppUser User { get; set; } = null!;
        public Trip Trip { get; set; } = null!;
    }
}
