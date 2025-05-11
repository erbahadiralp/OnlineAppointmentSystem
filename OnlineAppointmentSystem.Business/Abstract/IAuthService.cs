using OnlineAppointmentSystem.Entity.DTOs;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Abstract
{
    public interface IAuthService
    {
        Task<UserDTO> LoginAsync(LoginDTO loginDTO);
        Task<bool> RegisterAsync(RegisterDTO registerDTO);
        Task LogoutAsync();
        Task<bool> IsEmailInUseAsync(string email);
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> AddUserToRoleAsync(string userId, string roleName);
        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string userId, string token, string newPassword);
    }
}