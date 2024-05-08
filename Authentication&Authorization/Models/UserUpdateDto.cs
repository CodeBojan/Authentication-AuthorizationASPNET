using System.ComponentModel.DataAnnotations;

namespace Authentication_Authorization.Models
{
    public class UserUpdateDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string NewUsername { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }

        [RegularExpression("(?:^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$|)", ErrorMessage = "Email is not of valid format!")]
        public string NewEmail { get; set; }
    }
}
