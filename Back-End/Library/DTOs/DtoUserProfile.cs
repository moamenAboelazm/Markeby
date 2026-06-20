using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.DTOs
{
    public class DtoUserProfile
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfileImgUrl { get; set; }
        public bool IsActive { get; set; }
    }

    public class DtoUpdateProfile
    {
        public string FullName { get; set; }
        public string ProfileImgUrl { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class DtoChangePassword
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
