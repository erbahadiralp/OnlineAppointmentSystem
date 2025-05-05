using OnlineAppointmentSystem.Entity.DTOs;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Abstract
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDTO registerDTO);
        Task<UserDTO> LoginAsync(LoginDTO loginDTO);
        Task LogoutAsync();
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string userId, string token, string newPassword);
    }
}