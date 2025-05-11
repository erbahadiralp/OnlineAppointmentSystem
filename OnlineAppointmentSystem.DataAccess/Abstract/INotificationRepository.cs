using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Abstract
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetPendingNotificationsAsync();
        Task<IEnumerable<Notification>> GetNotificationsByAppointmentIdAsync(int appointmentId);
    }
}