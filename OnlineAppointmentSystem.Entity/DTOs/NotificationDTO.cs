using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Entity.DTOs
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }

        [Required(ErrorMessage = "Appointment is required")]
        public int AppointmentId { get; set; }
        public string AppointmentInfo { get; set; }

        [Required(ErrorMessage = "Notification type is required")]
        [StringLength(50, ErrorMessage = "Notification type cannot exceed 50 characters")]
        [Display(Name = "Notification Type")]
        public string NotificationType { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        public bool IsSent { get; set; }

        public DateTime? SentDate { get; set; }
    }
}