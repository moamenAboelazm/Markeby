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
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ExperienceInfo { get; set; } = string.Empty;

        public ICollection<Boat> Boats { get; set; } = new List<Boat>();
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
