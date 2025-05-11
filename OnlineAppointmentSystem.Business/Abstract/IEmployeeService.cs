using OnlineAppointmentSystem.Entity.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Abstract
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDTO>> GetAllEmployeesAsync();
        Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
        Task<EmployeeDTO> GetEmployeeByUserIdAsync(string userId);
        Task<List<EmployeeDTO>> GetActiveEmployeesAsync();
        Task<List<EmployeeDTO>> GetEmployeesByServiceIdAsync(int serviceId);
        Task<bool> CreateEmployeeAsync(EmployeeDTO employeeDTO);
        Task<bool> UpdateEmployeeAsync(EmployeeDTO employeeDTO);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<bool> ActivateEmployeeAsync(int id);
        Task<bool> DeactivateEmployeeAsync(int id);
        Task<bool> AssignServiceToEmployeeAsync(int employeeId, int serviceId);
        Task<bool> RemoveServiceFromEmployeeAsync(int employeeId, int serviceId);
    }
}