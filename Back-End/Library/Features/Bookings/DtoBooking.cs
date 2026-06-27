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
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public Guid TripId { get; set; }
        public string TripTitle { get; set; } = string.Empty;
        public int NumberOfTickets { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
        public string? SpecialRequests { get; set; }
        public string? CancellationReason { get; set; }
        public string Status => string.IsNullOrEmpty(CancellationReason) ? "Active" : "Cancelled";
    }

    public class SystemDashboardDto
    {
        public int TotalCompletedTrips { get; set; }
        public int TotalCancelledTrips { get; set; }
        public int TotalScheduledTrips { get; set; }
        public decimal TotalSystemProfit { get; set; }
    }

    public class TripDashboardDto
    {
        public int TicketsSold { get; set; }
        public int TicketsRemaining { get; set; }
        public decimal TotalTripProfit { get; set; }
    }

}
