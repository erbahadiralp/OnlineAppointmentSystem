using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public class WorkingHoursRepository : GenericRepository<WorkingHours>, IWorkingHoursRepository
    {
        public WorkingHoursRepository(OnlineAppointmentSystemDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<WorkingHours>> GetAllAsync()
        {
            return await _dbSet
                .Include(wh => wh.Employee)
                    .ThenInclude(e => e.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkingHours>> GetWorkingHoursByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet
                .Where(wh => wh.EmployeeId == employeeId && wh.IsActive)
                .OrderBy(wh => wh.DayOfWeek)
                .ThenBy(wh => wh.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkingHours>> GetWorkingHoursByDayOfWeekAsync(int dayOfWeek)
        {
            return await _dbSet
                .Include(wh => wh.Employee)
                    .ThenInclude(e => e.User)
                .Where(wh => wh.DayOfWeek == dayOfWeek && wh.IsActive)
                .OrderBy(wh => wh.Employee.User.LastName)
                .ThenBy(wh => wh.Employee.User.FirstName)
                .ThenBy(wh => wh.StartTime)
                .ToListAsync();
        }
    }
}