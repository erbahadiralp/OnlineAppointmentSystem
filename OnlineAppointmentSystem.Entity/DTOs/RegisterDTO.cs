using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Entity.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; }

        // Default role is Customer
        public string Role { get; set; } = "Customer";
    }
}