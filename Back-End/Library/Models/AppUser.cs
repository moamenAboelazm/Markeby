
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

        public enum GenderType { Male, Female }
        public GenderType Gender { get; set; }

        public enum NationalityType { Egyptian, Saudi, Moroccan, Tunisian, Algerian, Libyan, Sudanese, Another }
        public NationalityType Nationality { get; set; }

        public string? Address { get; set; }
        
        public enum Role { Admin , User }
        
        public double Rating { get; set; } = 5.00;
        
        public string? ProfileImgUrl { get; set; }


       
    }
}
