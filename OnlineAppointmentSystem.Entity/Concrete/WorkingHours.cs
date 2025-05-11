using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineAppointmentSystem.Entity.Concrete;

namespace OnlineAppointmentSystem.Entity.Concrete
{
    public class WorkingHours
    {
        [Key]
        public int WorkingHoursId { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [Range(0, 6, ErrorMessage = "Gün değeri 0-6 arasında olmalıdır")]
        public int DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Employee Employee { get; set; }
    }
}