using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public void Update(AppUser user)
        {
            _userManager.UpdateAsync(user).Wait();
        }

        // IGenericRepository<AppUser> implementation
        public Task<AppUser> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<System.Collections.Generic.IEnumerable<AppUser>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<System.Collections.Generic.IEnumerable<AppUser>> FindAsync(System.Linq.Expressions.Expression<System.Func<AppUser, bool>> expression)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppUser> SingleOrDefaultAsync(System.Linq.Expressions.Expression<System.Func<AppUser, bool>> predicate)
        {
            throw new System.NotImplementedException();
        }

        public Task AddAsync(AppUser entity)
        {
            throw new System.NotImplementedException();
        }

        public Task AddRangeAsync(System.Collections.Generic.IEnumerable<AppUser> entities)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateRange(System.Collections.Generic.IEnumerable<AppUser> entities)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(AppUser entity)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveRange(System.Collections.Generic.IEnumerable<AppUser> entities)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CountAsync(System.Linq.Expressions.Expression<System.Func<AppUser, bool>> predicate)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AnyAsync(System.Linq.Expressions.Expression<System.Func<AppUser, bool>> predicate)
        {
            throw new System.NotImplementedException();
        }

        public System.Linq.IQueryable<AppUser> Include(params System.Linq.Expressions.Expression<System.Func<AppUser, object>>[] includes)
        {
            throw new System.NotImplementedException();
        }
    }
}