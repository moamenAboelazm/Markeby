
using Library.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [MaxLength(15,ErrorMessage ="First Name length Must be less than or equal to 15")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(15, ErrorMessage = "Last Name length Must be less than or equal to 15")]
        public string LastName { get; set; } 

        [Required]
        public DateTime BirthDate { get; set; }

        public GenderType Gender { get; set; }

        public NationalityType Nationality { get; set; }

        public RoleType Role { get; set; }

        public string? Address { get; set; }
                
        public string? ProfileImgUrl { get; set; }


        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();

    }
}
