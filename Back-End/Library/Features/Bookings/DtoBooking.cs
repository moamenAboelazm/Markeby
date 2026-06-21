using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Bookings
{
    public class DtoBooking
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid TripId { get; set; }
        public int NumberOfTickets { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
        public string? SpecialRequests { get; set; }
        public string? CancellationReason { get; set; }
    }
}
