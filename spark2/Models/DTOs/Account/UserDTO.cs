namespace spark2.Models.DTOs.Account
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }  
        public byte[]? ProfilePicture { get; set; }
        public string Role { get; set; }
       
        public List<string> AvailableRoles { get; set; } = new List<string>();
    }
}
