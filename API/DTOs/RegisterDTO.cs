using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [RegularExpression("^(?=.{8,})(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[(!@#$%^&*()_+|~\\- =\\`{}[\\]:”;'<>?,.\\/, )])(?!.*(.)\\1{2,}).+$", ErrorMessage = "Password must be at least 8 characters long with at least one uppercase letter and one lowercase letter and one number and one symbol")]
        public string Password { get; set; }
    }
}