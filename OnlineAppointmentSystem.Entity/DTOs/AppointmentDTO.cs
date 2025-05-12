using OnlineAppointmentSystem.Entity.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Entity.DTOs
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Employee is required")]
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeTitle { get; set; }

        [Required(ErrorMessage = "Service is required")]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Appointment date is required")]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Status")]
        public AppointmentStatus Status { get; set; }
        public string StatusName { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public bool ReminderSent { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }
    }
}