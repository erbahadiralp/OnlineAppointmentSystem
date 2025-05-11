using OnlineAppointmentSystem.Entity.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Abstract
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(string id);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(UserDTO userDTO);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> AssignRoleToUserAsync(string userId, string roleName);
        Task<bool> RemoveRoleFromUserAsync(string userId, string roleName);
        Task<List<string>> GetUserRolesAsync(string userId);
    }
}