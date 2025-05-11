using OnlineAppointmentSystem.Entity.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Abstract
{
    public interface IWorkingHoursService
    {
        Task<List<WorkingHoursDTO>> GetAllWorkingHoursAsync();
        Task<WorkingHoursDTO> GetWorkingHoursByIdAsync(int id);
        Task<List<WorkingHoursDTO>> GetWorkingHoursByEmployeeIdAsync(int employeeId);
        Task<List<WorkingHoursDTO>> GetWorkingHoursByDayOfWeekAsync(int dayOfWeek);
        Task<bool> CreateWorkingHoursAsync(WorkingHoursDTO workingHoursDTO);
        Task<bool> UpdateWorkingHoursAsync(WorkingHoursDTO workingHoursDTO);
        Task<bool> DeleteWorkingHoursAsync(int id);
        Task<bool> IsEmployeeAvailableAtTimeAsync(int employeeId, int dayOfWeek, System.TimeSpan startTime, System.TimeSpan endTime);
    }
}