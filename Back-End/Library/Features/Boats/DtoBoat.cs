using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Boats
{
    public class DtoBoat
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? MainImageUrl { get; set; }
        public int YearBuilt { get; set; }
        public bool HasWifi { get; set; }
        public bool HasFoodFacility { get; set; }
        public bool HasToilet { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
