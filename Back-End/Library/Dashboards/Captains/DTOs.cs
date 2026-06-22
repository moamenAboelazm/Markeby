using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dashboards.Captains
{
    public class CaptainDashboardStatsDto
    {
        public int TotalCaptains { get; set; }
        public int OnMission { get; set; }
        public int OnShoreLeave { get; set; }
    }

    public class CaptainListDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public string Vessel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? ProfilePhotoUrl { get; set; }
    }
}
