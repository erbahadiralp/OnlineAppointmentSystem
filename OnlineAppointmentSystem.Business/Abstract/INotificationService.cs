using OnlineAppointmentSystem.Entity.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Abstract
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetAllNotificationsAsync();
        Task<NotificationDTO> GetNotificationByIdAsync(int id);
        Task<List<NotificationDTO>> GetNotificationsByAppointmentIdAsync(int appointmentId);
        Task<List<NotificationDTO>> GetPendingNotificationsAsync();
        Task<bool> CreateNotificationAsync(NotificationDTO notificationDTO);
        Task<bool> UpdateNotificationAsync(NotificationDTO notificationDTO);
        Task<bool> DeleteNotificationAsync(int id);
        Task<bool> SendEmailNotificationAsync(NotificationDTO notificationDTO);
        Task ProcessPendingNotificationsAsync();
    }
}