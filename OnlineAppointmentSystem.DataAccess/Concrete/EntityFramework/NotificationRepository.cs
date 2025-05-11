using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(OnlineAppointmentSystemDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetPendingNotificationsAsync()
        {
            return await _dbSet
                .Include(n => n.Appointment)
                    .ThenInclude(a => a.Customer)
                        .ThenInclude(c => c.User)
                .Include(n => n.Appointment)
                    .ThenInclude(a => a.Employee)
                        .ThenInclude(e => e.User)
                .Include(n => n.Appointment)
                    .ThenInclude(a => a.Service)
                .Where(n => !n.IsSent)
                .OrderBy(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByAppointmentIdAsync(int appointmentId)
        {
            return await _dbSet
                .Where(n => n.AppointmentId == appointmentId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }
    }
}