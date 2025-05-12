using OnlineAppointmentSystem.Entity.Concrete;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Abstract
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        Task<AppUser> GetUserByIdAsync(string id);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<AppUser> GetByIdAsync(string id);
        void Update(AppUser user);
    }
}