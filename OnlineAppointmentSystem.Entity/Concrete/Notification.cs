using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineAppointmentSystem.Entity.Concrete;

namespace OnlineAppointmentSystem.Entity.Concrete
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string NotificationType { get; set; } // Email, SMS

        [Required]
        public string Content { get; set; }

        public bool IsSent { get; set; }

        public DateTime? SentDate { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public virtual Appointment Appointment { get; set; }
    }
}