using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OnlineAppointmentSystem.Entity.Concrete;

namespace OnlineAppointmentSystem.Entity.Concrete
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required]
        [StringLength(100)]
        public string ServiceName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int Duration { get; set; } // Duration in minutes

        [Required]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<EmployeeService> EmployeeServices { get; set; }
    }
}