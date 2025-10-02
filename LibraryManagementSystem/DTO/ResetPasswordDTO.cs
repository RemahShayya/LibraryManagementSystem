using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.API.DTO
{
    public class ResetPasswordDTO
    {
        public string Token { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address!")]
        public string Email { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "New password must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}
