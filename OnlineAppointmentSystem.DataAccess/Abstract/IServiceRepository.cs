using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Abstract
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<IEnumerable<Service>> GetActiveServicesAsync();
        Task<IEnumerable<Service>> GetServicesByEmployeeIdAsync(int employeeId);
    }
}