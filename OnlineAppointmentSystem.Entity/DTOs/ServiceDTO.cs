using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Entity.DTOs
{
    public class ServiceDTO
    {
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Service name is required")]
        [StringLength(100, ErrorMessage = "Service name cannot exceed 100 characters")]
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
        [Display(Name = "Duration (minutes)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public List<EmployeeDTO> Employees { get; set; }
    }

}