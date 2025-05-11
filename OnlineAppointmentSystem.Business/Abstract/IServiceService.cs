using OnlineAppointmentSystem.Entity.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Abstract
{
    public interface IServiceService
    {
        Task<List<ServiceDTO>> GetAllServicesAsync();
        Task<ServiceDTO> GetServiceByIdAsync(int id);
        Task<List<ServiceDTO>> GetActiveServicesAsync();
        Task<List<ServiceDTO>> GetServicesByEmployeeIdAsync(int employeeId);
        Task<bool> CreateServiceAsync(ServiceDTO serviceDTO);
        Task<bool> UpdateServiceAsync(ServiceDTO serviceDTO);
        Task<bool> DeleteServiceAsync(int id);
        Task<bool> ActivateServiceAsync(int id);
        Task<bool> DeactivateServiceAsync(int id);
    }
}