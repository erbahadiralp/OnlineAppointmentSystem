using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.DataAccess.Abstract;
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

        public async Task<List<Employee>> GetActiveEmployeesAsync()
        {
            return await _dbSet
                .Include(e => e.User)
                .Where(e => e.IsActive)
                .OrderBy(e => e.User.LastName)
                .ThenBy(e => e.User.FirstName)
                .ToListAsync();
        }

        public async Task<List<Employee>> GetEmployeesByServiceIdAsync(int serviceId)
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

        public async Task<List<Employee>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(e => e.User)
                .Include(e => e.EmployeeServices)
                    .ThenInclude(es => es.Service)
                .Include(e => e.WorkingHours)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeWithServiceAsync(int employeeId, int serviceId)
        {
            return await _dbSet
                .Include(e => e.User)
                .Include(e => e.EmployeeServices)
                    .ThenInclude(es => es.Service)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId &&
                    e.EmployeeServices.Any(es => es.ServiceId == serviceId));
        }

        public async Task<Employee> GetEmployeeWithServicesAsync(int employeeId)
        {
            return await _dbSet
                .Include(e => e.User)
                .Include(e => e.EmployeeServices)
                    .ThenInclude(es => es.Service)
                .Include(e => e.WorkingHours)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }
    }
}
