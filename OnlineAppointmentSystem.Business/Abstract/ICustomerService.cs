using OnlineAppointmentSystem.Entity.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Abstract
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task<CustomerDTO> GetCustomerByUserIdAsync(string userId);
        Task<bool> CreateCustomerAsync(CustomerDTO customerDTO);
        Task<bool> UpdateCustomerAsync(CustomerDTO customerDTO);
        Task<bool> DeleteCustomerAsync(int id);
        Task<bool> ActivateCustomerAsync(int id);
        Task<bool> DeactivateCustomerAsync(int id);
    }
}