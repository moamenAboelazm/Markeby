using Library.Enums;
using Library.Models;

namespace Library.Features.Users
{
    public class DtoGetUser
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ProfileImgUrl { get; set; } = string.Empty;
        public int Age { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
    public class DtoGetUserProfile : DtoGetUser
    {
        public IEnumerable<DtoBookingsInProfile> Bookings { get; set; } = new List<DtoBookingsInProfile>();
    }

    public class DtoBookingsInProfile
    {
        public string Id { get; set; }
        public string TripTitle { get; set; }
        public DateTime StartDate { get; set; }
        public TripStatus TripStatus { get; set; }
        public double TotalPrice { get; set; }
    }

}
