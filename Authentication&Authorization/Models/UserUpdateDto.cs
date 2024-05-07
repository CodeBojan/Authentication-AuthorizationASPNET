namespace Authentication_Authorization.Models
{
    public class UserUpdateDto
    {
        public string Username { get; set; }
        public string NewUsername { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public string NewEmail { get; set; }
    }
}
