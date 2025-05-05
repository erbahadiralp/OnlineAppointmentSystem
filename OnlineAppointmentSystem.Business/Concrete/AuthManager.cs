using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using System;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        private readonly IEmployeeService _employeeService;

        public AuthManager(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            ICustomerService customerService,
            IEmployeeService employeeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _customerService = customerService;
            _employeeService = employeeService;
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
                var user = new AppUser
                {
                    UserName = registerDTO.Email,
                    Email = registerDTO.Email,
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    PhoneNumber = registerDTO.PhoneNumber,
                    Address = registerDTO.Address,
                    EmailConfirmed = true, // Gerçek uygulamada false olmalı ve e-posta doğrulaması yapılmalı
                    CreatedDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, registerDTO.Password);
                if (!result.Succeeded)
                    return false;

                // Kullanıcıya rol atama
                await _userManager.AddToRoleAsync(user, registerDTO.Role);

                // Rol'e göre müşteri veya çalışan oluştur
                if (registerDTO.Role == "Customer")
                {
                    var customerDTO = new CustomerDTO
                    {
                        UserId = user.Id,
                        DateOfBirth = null,
                        Gender = null
                    };

                    await _customerService.CreateCustomerAsync(customerDTO);
                }
                else if (registerDTO.Role == "Employee")
                {
                    var employeeDTO = new EmployeeDTO
                    {
                        UserId = user.Id,
                        Title = "Yeni Çalışan",
                        Department = "Genel"
                    };

                    await _employeeService.CreateEmployeeAsync(employeeDTO);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<UserDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
                return null;

            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.RememberMe, false);
            if (!result.Succeeded)
                return null;

            var userDTO = _mapper.Map<UserDTO>(user);
            var roles = await _userManager.GetRolesAsync(user);
            userDTO.Role = roles.Count > 0 ? roles[0] : null;

            return userDTO;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                return false;

            // Gerçek uygulamada, şifre sıfırlama token'ı oluşturulup e-posta gönderilmeli
            // var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // await _emailService.SendPasswordResetEmailAsync(user.Email, token);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
    }
}