using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Business.BackgroundServices;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineAppointmentSystem.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomerService _customerService;
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<AuthManager> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IServiceProvider _serviceProvider;

        public AuthManager(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ICustomerService customerService,
            IEmployeeService employeeService,
            ILogger<AuthManager> logger,
            IConfiguration configuration,
            IEmailService emailService,
            IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _customerService = customerService;
            _employeeService = employeeService;
            _logger = logger;
            _configuration = configuration;
            _emailService = emailService;
            _serviceProvider = serviceProvider;
        }

        //public async Task<UserDTO> LoginAsync(LoginDTO loginDTO)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Giriş denemesi: {loginDTO.Email}");

        //        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        //        if (user == null)
        //        {
        //            _logger.LogWarning($"Kullanıcı bulunamadı: {loginDTO.Email}");
        //            return null;
        //        }

        //        // Kullanıcının hesap durumunu kontrol et
        //        if (!user.EmailConfirmed && _userManager.Options.SignIn.RequireConfirmedEmail)
        //        {
        //            _logger.LogWarning($"Kullanıcının e-postası doğrulanmamış: {loginDTO.Email}");
        //            return null;
        //        }

        //        if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.Now)
        //        {
        //            _logger.LogWarning($"Kullanıcı hesabı kilitli: {loginDTO.Email}");
        //            return null;
        //        }

        //        // AppUser nesnesi ile doğrudan giriş dene
        //        var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.RememberMe, false);

        //        // Başarısız olursa, kullanıcı adı ve şifre ile dene
        //        if (!result.Succeeded)
        //        {
        //            _logger.LogWarning($"AppUser ile giriş başarısız, kullanıcı adı ile deneniyor: {loginDTO.Email}");
        //            result = await _signInManager.PasswordSignInAsync(user.UserName, loginDTO.Password, loginDTO.RememberMe, false);
        //        }

        //        // Hala başarısız olursa, e-posta ve şifre ile dene
        //        if (!result.Succeeded)
        //        {
        //            _logger.LogWarning($"Kullanıcı adı ile giriş başarısız, e-posta ile deneniyor: {loginDTO.Email}");
        //            result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, loginDTO.RememberMe, false);
        //        }

        //        // Şifre doğrulamasını manuel olarak kontrol et
        //        if (!result.Succeeded)
        //        {
        //            _logger.LogWarning($"Standart giriş yöntemleri başarısız, manuel şifre doğrulama deneniyor: {loginDTO.Email}");
        //            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
        //            if (passwordValid)
        //            {
        //                // Şifre doğru ama giriş başarısız, manuel olarak giriş yap
        //                await _signInManager.SignInAsync(user, loginDTO.RememberMe);
        //                result = Microsoft.AspNetCore.Identity.SignInResult.Success;
        //            }
        //        }

        //        if (result.Succeeded)
        //        {
        //            _logger.LogInformation($"Kullanıcı başarıyla giriş yaptı: {loginDTO.Email}");

        //            // Kullanıcının Employee rolünde olup olmadığını kontrol et
        //            bool hasEmployeeRole = await _userManager.IsInRoleAsync(user, "Employee");

        //            // UserDTO oluştur ve döndür
        //            return new UserDTO
        //            {
        //                Id = user.Id,
        //                FirstName = user.FirstName,
        //                LastName = user.LastName,
        //                Email = user.Email,
        //                PhoneNumber = user.PhoneNumber,
        //                Address = user.Address,
        //                CreatedDate = user.CreatedDate,
        //                HasEmployeeProfile = hasEmployeeRole
        //            };
        //        }
        //        else
        //        {
        //            // Başarısızlık nedenini detaylı loglama
        //            string failureReason = "Bilinmeyen hata";
        //            if (result.IsLockedOut)
        //                failureReason = "Hesap kilitlendi";
        //            else if (result.IsNotAllowed)
        //                failureReason = "Giriş izni yok";
        //            else if (result.RequiresTwoFactor)
        //                failureReason = "İki faktörlü doğrulama gerekiyor";
        //            else
        //                failureReason = "Geçersiz şifre";

        //            _logger.LogWarning($"Giriş başarısız: {loginDTO.Email}, Neden: {failureReason}");
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Giriş sırasında hata oluştu: {loginDTO.Email}");
        //        return null;
        //    }
        //}

        //public async Task<bool> RegisterAsync(RegisterDTO registerDTO)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Kayıt denemesi: {registerDTO.Email}");

        //        // Kullanıcı zaten var mı kontrol et
        //        var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
        //        if (existingUser != null)
        //        {
        //            _logger.LogWarning($"Email zaten kullanımda: {registerDTO.Email}");
        //            return false;
        //        }

        //        // Rol var mı kontrol et, yoksa oluştur
        //        if (!await _roleManager.RoleExistsAsync(registerDTO.Role))
        //        {
        //            _logger.LogInformation($"Rol oluşturuluyor: {registerDTO.Role}");
        //            var roleResult = await _roleManager.CreateAsync(new IdentityRole(registerDTO.Role));
        //            if (!roleResult.Succeeded)
        //            {
        //                string errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
        //                _logger.LogError($"Rol oluşturulurken hata: {registerDTO.Role}, Hatalar: {errors}");
        //                return false;
        //            }
        //        }

        //        // Yeni kullanıcı oluştur
        //        var user = new AppUser
        //        {
        //            UserName = registerDTO.Email,
        //            Email = registerDTO.Email,
        //            PhoneNumber = registerDTO.PhoneNumber,
        //            FirstName = registerDTO.FirstName,
        //            LastName = registerDTO.LastName,
        //            Address = registerDTO.Address,
        //            CreatedDate = DateTime.Now
        //        };

        //        // Kullanıcıyı kaydet
        //        _logger.LogInformation($"Kullanıcı oluşturuluyor: {registerDTO.Email}");
        //        var result = await _userManager.CreateAsync(user, registerDTO.Password);
        //        if (!result.Succeeded)
        //        {
        //            string errors = string.Join(", ", result.Errors.Select(e => e.Description));
        //            _logger.LogError($"Kullanıcı oluşturulurken hata: {registerDTO.Email}, Hatalar: {errors}");
        //            return false;
        //        }

        //        // Kullanıcıya rol ata
        //        _logger.LogInformation($"Kullanıcıya rol atanıyor: {registerDTO.Email}, Rol: {registerDTO.Role}");
        //        var roleAssignResult = await _userManager.AddToRoleAsync(user, registerDTO.Role);
        //        if (!roleAssignResult.Succeeded)
        //        {
        //            string errors = string.Join(", ", roleAssignResult.Errors.Select(e => e.Description));
        //            _logger.LogError($"Rol atanırken hata: {registerDTO.Email}, Rol: {registerDTO.Role}, Hatalar: {errors}");

        //            // Rol ataması başarısız olursa kullanıcıyı sil
        //            await _userManager.DeleteAsync(user);

        //            return false;
        //        }

        //        // Rol'e göre müşteri veya çalışan profili oluştur
        //        if (registerDTO.Role == "Customer")
        //        {
        //            _logger.LogInformation($"Müşteri profili oluşturuluyor: {registerDTO.Email}");
        //            var customerDTO = new CustomerDTO
        //            {
        //                UserId = user.Id,
        //                FirstName = user.FirstName,
        //                LastName = user.LastName,
        //                Email = user.Email,
        //                PhoneNumber = user.PhoneNumber,
        //                Address = user.Address,
        //                IsActive = true,
        //                CreatedDate = DateTime.Now
        //            };

        //            var customerResult = await _customerService.CreateCustomerAsync(customerDTO);
        //            if (!customerResult)
        //            {
        //                _logger.LogError($"Müşteri profili oluşturulurken hata: {registerDTO.Email}");

        //                // Müşteri profili oluşturulamazsa kullanıcıyı sil
        //                await _userManager.DeleteAsync(user);

        //                return false;
        //            }
        //        }
        //        else if (registerDTO.Role == "Employee")
        //        {
        //            _logger.LogInformation($"Çalışan rolü atandı, profil daha sonra oluşturulacak: {registerDTO.Email}");
        //            // Çalışan profili oluşturma işlemi burada yapılmaz
        //            // Çalışan profili admin tarafından oluşturulur
        //        }

        //        _logger.LogInformation($"Kullanıcı başarıyla kaydedildi: {registerDTO.Email}");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Kayıt sırasında beklenmeyen hata: {registerDTO.Email}");
        //        return false;
        //    }
        //}

        //public async Task LogoutAsync()
        //{
        //    try
        //    {
        //        _logger.LogInformation("Kullanıcı çıkış yapıyor");
        //        await _signInManager.SignOutAsync();
        //        _logger.LogInformation("Kullanıcı başarıyla çıkış yaptı");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Çıkış sırasında hata oluştu");
        //        throw;
        //    }
        //}

        public async Task<UserDTO> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı: {loginDTO.Email}");
                    return null;
                }

                // Kullanıcı hesabı kilitli mi kontrol et
                if (await _userManager.IsLockedOutAsync(user))
                {
                    _logger.LogWarning($"Kullanıcı hesabı kilitli: {loginDTO.Email}");
                    return null;
                }

                // Kullanıcının rollerini kontrol et
                var roles = await _userManager.GetRolesAsync(user);
                bool isAdminOrEmployee = roles.Contains("Admin") || roles.Contains("Employee");

                // Şifre doğrulama
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (!passwordCheck)
                {
                    _logger.LogWarning($"Geçersiz şifre: {loginDTO.Email}");
                    // Başarısız giriş sayısını artır
                    await _userManager.AccessFailedAsync(user);
                    return null;
                }

                // Tüm kullanıcı türleri için başarılı şifre doğrulaması sonrası direkt giriş yap
                // Add claims for name
                var claims = new List<Claim>
                {
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName)
                };
                
                // First create the identity principal with claims
                await _userManager.AddClaimsAsync(user, claims);
                
                // Then sign in the user
                await _signInManager.SignInAsync(user, loginDTO.RememberMe);
            
                // E-posta doğrulama durumunu al - Customer için uyarı için kullanacağız
                bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            
                _logger.LogInformation($"Kullanıcı başarıyla giriş yaptı: {loginDTO.Email}");
            
                // Başarılı giriş sonrası kullanıcı bilgilerini döndür
                return new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = roles.Count > 0 ? roles[0] : null,
                    IsEmailConfirmed = isEmailConfirmed // E-posta doğrulama durumunu da döndür
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Giriş sırasında hata oluştu: {loginDTO.Email}");
                throw;
            }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
                // Email adresi zaten kullanılıyor mu kontrol et
                var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning($"Email adresi zaten kullanılıyor: {registerDTO.Email}");
                    return false;
                }

                // Format phone number to include +90 prefix
                string formattedPhoneNumber = FormatPhoneNumber(registerDTO.PhoneNumber);

                var user = new AppUser
                {
                    UserName = registerDTO.Email,
                    Email = registerDTO.Email,
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    PhoneNumber = formattedPhoneNumber,
                    Address = registerDTO.Address
                };

                var result = await _userManager.CreateAsync(user, registerDTO.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Kullanıcı başarıyla oluşturuldu: {registerDTO.Email}");

                    // Kullanıcıya rol ata
                    if (!string.IsNullOrEmpty(registerDTO.Role))
                    {
                        await _userManager.AddToRoleAsync(user, registerDTO.Role);
                    }
                    else
                    {
                        // Varsayılan rol
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    return true;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogWarning($"Kullanıcı oluşturma hatası: {error.Description}");
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Kayıt sırasında hata oluştu: {registerDTO.Email}");
                return false;
            }
        }

        // Format phone number to ensure it starts with +90
        private string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber;

            // Remove all non-digit characters
            phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // If number starts with 0, remove it
            if (phoneNumber.StartsWith("0"))
                phoneNumber = phoneNumber.Substring(1);

            // Ensure the number starts with +90
            if (!phoneNumber.StartsWith("90"))
                phoneNumber = "90" + phoneNumber;

            return "+" + phoneNumber;
        }

        public async Task<bool> IsEmailInUseAsync(string email)
        {
            try
            {
                _logger.LogInformation($"Email kullanımda mı kontrol ediliyor: {email}");
                var user = await _userManager.FindByEmailAsync(email);
                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Email kontrolü sırasında hata: {email}");
                throw;
            }
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            try
            {
                _logger.LogInformation($"Rol oluşturma denemesi: {roleName}");

                if (await _roleManager.RoleExistsAsync(roleName))
                {
                    _logger.LogWarning($"Rol zaten var: {roleName}");
                    return true;
                }

                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Rol başarıyla oluşturuldu: {roleName}");
                    return true;
                }
                else
                {
                    string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError($"Rol oluşturulurken hata: {roleName}, Hatalar: {errors}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Rol oluşturma sırasında beklenmeyen hata: {roleName}");
                return false;
            }
        }

        public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
        {
            try
            {
                _logger.LogInformation($"Kullanıcıya rol ekleme denemesi: UserId={userId}, Rol={roleName}");

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı: {userId}");
                    return false;
                }

                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    _logger.LogWarning($"Rol bulunamadı: {roleName}");
                    return false;
                }

                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                    _logger.LogWarning($"Kullanıcı zaten bu role sahip: UserId={userId}, Rol={roleName}");
                    return true;
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Kullanıcıya rol başarıyla eklendi: UserId={userId}, Rol={roleName}");
                    return true;
                }
                else
                {
                    string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError($"Kullanıcıya rol eklenirken hata: UserId={userId}, Rol={roleName}, Hatalar: {errors}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Kullanıcıya rol ekleme sırasında beklenmeyen hata: UserId={userId}, Rol={roleName}");
                return false;
            }
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            try
            {
                _logger.LogInformation($"Kullanıcıdan rol kaldırma denemesi: UserId={userId}, Rol={roleName}");

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı: {userId}");
                    return false;
                }

                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                    _logger.LogWarning($"Kullanıcı bu role sahip değil: UserId={userId}, Rol={roleName}");
                    return true;
                }

                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Kullanıcıdan rol başarıyla kaldırıldı: UserId={userId}, Rol={roleName}");
                    return true;
                }
                else
                {
                    string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError($"Kullanıcıdan rol kaldırılırken hata: UserId={userId}, Rol={roleName}, Hatalar: {errors}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Kullanıcıdan rol kaldırma sırasında beklenmeyen hata: UserId={userId}, Rol={roleName}");
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            try
            {
                _logger.LogInformation($"Şifre değiştirme denemesi: UserId={userId}");

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı: {userId}");
                    return false;
                }

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Şifre başarıyla değiştirildi: UserId={userId}");
                    return true;
                }
                else
                {
                    string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError($"Şifre değiştirilirken hata: UserId={userId}, Hatalar: {errors}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Şifre değiştirme sırasında beklenmeyen hata: UserId={userId}");
                return false;
            }
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            try
            {
                _logger.LogInformation($"Email onaylama denemesi: UserId={userId}");

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı: {userId}");
                    return false;
                }

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Email başarıyla onaylandı: UserId={userId}");
                    return true;
                }
                else
                {
                    string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError($"Email onaylanırken hata: UserId={userId}, Hatalar: {errors}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Email onaylama sırasında beklenmeyen hata: UserId={userId}");
                return false;
            }
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            try
            {
                _logger.LogInformation($"Şifre sıfırlama talebi: {email}");

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı: {email}");
                    return false;
                }

                // Şifre sıfırlama token'ı oluştur
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Gömülü URL'ye göre şifre yenileme bağlantısı
                var callbackUrl = $"https://localhost:55675/Account/ResetPassword?userId={user.Id}&token={Uri.EscapeDataString(token)}";

                // HTML e-posta içeriği
                var emailBody = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px; }}
                        .header {{ color: #4a86e8; }}
                        .button {{ display: inline-block; padding: 10px 20px; background-color: #4a86e8; color: white; text-decoration: none; border-radius: 4px; }}
                        .footer {{ margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0; font-size: 12px; color: #777; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2 class='header'>Merhaba {user.FirstName} {user.LastName},</h2>
                        
                        <p>Şifrenizi sıfırlamak için talepte bulundunuz.</p>
                        
                        <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:</p>
                        
                        <p><a href='{callbackUrl}' class='button'>Şifremi Sıfırla</a></p>
                        
                        <p>Eğer bu talebi siz yapmadıysanız, lütfen bu e-postayı dikkate almayın.</p>
                        
                        <p>Bağlantı 24 saat boyunca geçerli olacaktır.</p>
                        
                        <div class='footer'>
                            <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
                        </div>
                    </div>
                </body>
                </html>";

                // E-postayı kuyruğa ekle (arka planda gönderilecek)
                var emailQueueService = _serviceProvider.GetRequiredService<EmailQueueService>();
                emailQueueService.QueueEmail(
                    email,
                    "Şifre Sıfırlama Talebi",
                    emailBody);

                _logger.LogInformation($"Şifre sıfırlama e-postası kuyruğa eklendi: {email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Şifre sıfırlama talebi sırasında beklenmeyen hata: {email}");
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            try
            {
                _logger.LogInformation($"Şifre sıfırlama denemesi: UserId={userId}");

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı: {userId}");
                    return false;
                }

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Şifre başarıyla sıfırlandı: UserId={userId}");
                    return true;
                }
                else
                {
                    string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError($"Şifre sıfırlanırken hata: UserId={userId}, Hatalar: {errors}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Şifre sıfırlama sırasında beklenmeyen hata: UserId={userId}");
                return false;
            }
        }
    }
}