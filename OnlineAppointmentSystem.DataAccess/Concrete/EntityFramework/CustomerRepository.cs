using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(OnlineAppointmentSystemDbContext context) : base(context)
        {
        }

        public async Task<Customer> GetCustomerWithUserByIdAsync(int customerId)
        {
            return await _dbSet
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<Customer> GetCustomerByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<List<Customer>> GetAllWithUserAsync()
        {
            return await _dbSet.Include(c => c.User).ToListAsync();
        }
    }
}