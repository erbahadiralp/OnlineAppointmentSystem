using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(OnlineAppointmentSystemDbContext context) : base(context)
        {
        }

        public override async Task<Appointment> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Customer)
                    .ThenInclude(c => c.User)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.User)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Include(a => a.Employee)
                    .ThenInclude(e => e.User)
                .Include(a => a.Service)
                .Where(a => a.CustomerId == customerId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet
                .Include(a => a.Customer)
                    .ThenInclude(c => c.User)
                .Include(a => a.Service)
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(a => a.Customer)
                    .ThenInclude(c => c.User)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.User)
                .Include(a => a.Service)
                .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status)
        {
            return await _dbSet
                .Include(a => a.Customer)
                    .ThenInclude(c => c.User)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.User)
                .Include(a => a.Service)
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync()
        {
            var now = DateTime.Now;
            return await _dbSet
                .Include(a => a.Customer)
                    .ThenInclude(c => c.User)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.User)
                .Include(a => a.Service)
                .Where(a => a.AppointmentDate > now && a.Status == AppointmentStatus.Confirmed)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsForReminderAsync()
        {
            var now = DateTime.Now;
            var reminderTime = now.AddHours(24);
            return await _dbSet
                .Include(a => a.Customer)
                    .ThenInclude(c => c.User)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.User)
                .Include(a => a.Service)
                .Where(a => 
                    a.AppointmentDate > now && 
                    a.AppointmentDate <= reminderTime && 
                    a.Status == AppointmentStatus.Confirmed && 
                    !a.ReminderSent)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _dbSet
                .Include(a => a.Customer)
                    .ThenInclude(c => c.User)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.User)
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }
    }
}