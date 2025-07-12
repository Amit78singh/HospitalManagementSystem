using System.ComponentModel.DataAnnotations;

namespace HospitalManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required (ErrorMessage ="Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is Required")]
        public string? Role { get; set; } = "User"; // Default role is User
    }
}
