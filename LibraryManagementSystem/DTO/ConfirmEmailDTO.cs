using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.API.DTO
{
    public class ConfirmEmailDTO
    {
        public string Token { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address!")]
        public string Email { get; set; }
    }
}
