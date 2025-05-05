using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(OnlineAppointmentSystemDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        {
            return await _dbSet
                .Include(e => e.User)
                .Where(e => e.IsActive)
                .OrderBy(e => e.User.LastName)
            .ThenBy(e => e.User.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByServiceIdAsync(int serviceId)
        {
            return await _context.EmployeeServices
                .Where(es => es.ServiceId == serviceId)
                .Include(es => es.Employee)
                    .ThenInclude(e => e.User)
                .Select(es => es.Employee)
                .Where(e => e.IsActive)
                .OrderBy(e => e.User.LastName)
                .ThenBy(e => e.User.FirstName)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeWithUserByIdAsync(int employeeId)
        {
            return await _dbSet
                .Include(e => e.User)
                .Include(e => e.EmployeeServices)
                    .ThenInclude(es => es.Service)
                .Include(e => e.WorkingHours)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task<Employee> GetEmployeeByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(e => e.User)
                .Include(e => e.EmployeeServices)
                    .ThenInclude(es => es.Service)
                .Include(e => e.WorkingHours)
                .FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task<bool> CheckEmployeeServiceExistsAsync(int employeeId, int serviceId)
        {
            return await _context.EmployeeServices
                .AnyAsync(es => es.EmployeeId == employeeId && es.ServiceId == serviceId);
        }

        public Task<List<Employee>> GetAllWithDetailsAsync()
        {
            throw new NotImplementedException();
        }

        Task<List<Employee>> IEmployeeRepository.GetActiveEmployeesAsync()
        {
            throw new NotImplementedException();
        }

        Task<List<Employee>> IEmployeeRepository.GetEmployeesByServiceIdAsync(int serviceId)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetEmployeeWithServiceAsync(int employeeId, int serviceId)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetEmployeeWithServicesAsync(int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}