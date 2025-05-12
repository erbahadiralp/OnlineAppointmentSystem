using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineAppointmentSystem.Entity.Concrete;

namespace OnlineAppointmentSystem.Entity.Concrete
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}