using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Abstract
{
    public interface IWorkingHoursRepository : IGenericRepository<WorkingHours>
    {
        Task<IEnumerable<WorkingHours>> GetWorkingHoursByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<WorkingHours>> GetWorkingHoursByDayOfWeekAsync(int dayOfWeek);
    }
}