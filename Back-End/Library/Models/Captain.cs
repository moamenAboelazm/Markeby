using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Captain
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public string Rank { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public string? ProfilePhotoUrl { get; set; }

        public ICollection<Boat> Boats { get; set; } = new List<Boat>();
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
