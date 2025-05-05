using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public class EmployeeServiceRepository : GenericRepository<EmployeeService>, IEmployeeServiceRepository
    {
        public EmployeeServiceRepository(OnlineAppointmentSystemDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsAsync(int employeeId, int serviceId)
        {
            return await _dbSet.AnyAsync(es => es.EmployeeId == employeeId && es.ServiceId == serviceId);
        }

        public async Task<List<EmployeeService>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet
                .Include(es => es.Service)
                .Where(es => es.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<List<EmployeeService>> GetByServiceIdAsync(int serviceId)
        {
            return await _dbSet
                .Include(es => es.Employee)
                .Where(es => es.ServiceId == serviceId)
                .ToListAsync();
        }
    }
}