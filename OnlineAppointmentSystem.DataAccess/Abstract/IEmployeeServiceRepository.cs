using OnlineAppointmentSystem.Entity.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Abstract
{
    public interface IEmployeeServiceRepository : IGenericRepository<EmployeeService>
    {
        Task<bool> ExistsAsync(int employeeId, int serviceId);
        Task<List<EmployeeService>> GetByEmployeeIdAsync(int employeeId);
        Task<List<EmployeeService>> GetByServiceIdAsync(int serviceId);
    }
}