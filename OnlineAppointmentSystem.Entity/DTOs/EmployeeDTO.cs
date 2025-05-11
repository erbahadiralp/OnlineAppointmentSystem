using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Entity.DTOs
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
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

        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters")]
        public string Department { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public List<int> ServiceIds { get; set; } = new List<int>();
        public List<string> ServiceNames { get; set; } = new List<string>();
    }
}