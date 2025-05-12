using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OnlineAppointmentSystem.DataAccess.Abstract
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetCustomerWithUserByIdAsync(int customerId);
        Task<Customer> GetCustomerByUserIdAsync(string userId);
        Task<List<Customer>> GetAllWithUserAsync();
    }
}