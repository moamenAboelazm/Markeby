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
}
