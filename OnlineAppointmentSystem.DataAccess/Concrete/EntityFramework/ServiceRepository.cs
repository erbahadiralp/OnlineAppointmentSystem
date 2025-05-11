using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(OnlineAppointmentSystemDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Service>> GetActiveServicesAsync()
        {
            return await _dbSet
                .Include(s => s.EmployeeServices)
                    .ThenInclude(es => es.Employee)
                        .ThenInclude(e => e.User)
                .Where(s => s.IsActive)
                .OrderBy(s => s.ServiceName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetServicesByEmployeeIdAsync(int employeeId)
        {
            return await _context.EmployeeServices
                .Where(es => es.EmployeeId == employeeId)
                .Include(es => es.Service)
                .Select(es => es.Service)
                .OrderBy(s => s.ServiceName)
                .ToListAsync();
        }
    }
}