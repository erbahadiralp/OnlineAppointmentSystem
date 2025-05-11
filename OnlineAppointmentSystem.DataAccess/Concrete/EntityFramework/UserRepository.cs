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
    }
}