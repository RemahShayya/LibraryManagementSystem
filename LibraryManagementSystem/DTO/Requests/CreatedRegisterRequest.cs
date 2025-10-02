using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.API.Controllers
{
    public class CreatedRegisterRequest
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "First name must be at least 3 and at most 15 characters long!")]
        public string Firstname { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Last name must be at least 3 and at most 15 characters long!")]
        public string Lastname { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage ="Invalid Email Address!")]
        public string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be at least 6 and at most 15 characters long!")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}