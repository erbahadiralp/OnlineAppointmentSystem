using OnlineAppointmentSystem.Entity.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Abstract
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<List<Employee>> GetAllWithDetailsAsync();
        Task<Employee> GetEmployeeWithUserByIdAsync(int id);
        Task<Employee> GetEmployeeByUserIdAsync(string userId);
        Task<List<Employee>> GetActiveEmployeesAsync();
        Task<List<Employee>> GetEmployeesByServiceIdAsync(int serviceId);
        Task<Employee> GetEmployeeWithServiceAsync(int employeeId, int serviceId);
        Task<Employee> GetEmployeeWithServicesAsync(int employeeId);
        Task<bool> CheckEmployeeServiceExistsAsync(int employeeId, int serviceId);
    }
}