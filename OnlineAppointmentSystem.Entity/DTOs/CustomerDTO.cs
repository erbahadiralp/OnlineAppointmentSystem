using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Entity.DTOs
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }
        public string UserId { get; set; }

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

        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}