using System.ComponentModel.DataAnnotations;

namespace server.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        [Display(Name = "Username/Email")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; } = false;
    }
}