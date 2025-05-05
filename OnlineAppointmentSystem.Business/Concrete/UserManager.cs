using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserManager(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var userDTOs = _mapper.Map<List<UserDTO>>(users);

            foreach (var userDTO in userDTOs)
            {
                var user = await _userManager.FindByIdAsync(userDTO.Id);
                var roles = await _userManager.GetRolesAsync(user);
                userDTO.Role = roles.Count > 0 ? roles[0] : null;
            }

            return userDTOs;
        }

        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;

            var userDTO = _mapper.Map<UserDTO>(user);
            var roles = await _userManager.GetRolesAsync(user);
            userDTO.Role = roles.Count > 0 ? roles[0] : null;

            return userDTO;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var userDTO = _mapper.Map<UserDTO>(user);
            var roles = await _userManager.GetRolesAsync(user);
            userDTO.Role = roles.Count > 0 ? roles[0] : null;

            return userDTO;
        }

        public async Task<bool> UpdateUserAsync(UserDTO userDTO)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userDTO.Id);
                if (user == null)
                    return false;

                user.FirstName = userDTO.FirstName;
                user.LastName = userDTO.LastName;
                user.Email = userDTO.Email;
                user.UserName = userDTO.Email;
                user.PhoneNumber = userDTO.PhoneNumber;
                user.Address = userDTO.Address;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return false;

                // Rol değiştiyse güncelle
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Count > 0 && currentRoles[0] != userDTO.Role)
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, userDTO.Role);
                }
                else if (currentRoles.Count == 0 && !string.IsNullOrEmpty(userDTO.Role))
                {
                    await _userManager.AddToRoleAsync(user, userDTO.Role);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return false;

                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                return result.Succeeded;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                var result = await _userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveRoleFromUserAsync(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                return result.Succeeded;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new List<string>();

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
    }
}