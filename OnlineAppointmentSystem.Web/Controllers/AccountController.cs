using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Business.BackgroundServices;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Web.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICustomerService _customerService;

        public AccountController(
            IAuthService authService,
            IUserService userService,
            ILogger<AccountController> logger,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ICustomerService customerService)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _customerService = customerService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            // Eğer kullanıcı zaten giriş yapmışsa, ana sayfaya yönlendir
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var loginDTO = new LoginDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    RememberMe = model.RememberMe
                };

                try
                {
                    // Önce kullanıcıyı bul
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                        return View(model);
                    }

                    // AuthManager sınıfında yaptığımız düzenleme sayesinde e-posta doğrulama kontrolüne gerek yok
                    // Kullanıcılar doğrulanmış e-posta olmadan da giriş yapabilir, sadece randevu oluşturma gibi
                    // belirli işlemler için e-posta doğrulaması gerekli olacak

                    // Giriş işlemini dene
                    var result = await _authService.LoginAsync(loginDTO);
                    if (result != null)
                    {
                        _logger.LogInformation($"Kullanıcı başarıyla giriş yaptı: {model.Email}");

                        // Özel bir dönüş URL'si varsa oraya yönlendir, yoksa ana sayfaya
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        _logger.LogWarning($"Başarısız giriş denemesi: {model.Email}");
                        ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Giriş sırasında hata: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Giriş sırasında bir hata oluştu.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    CreatedDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Kullanıcıya Customer rolünü ata
                    await _userManager.AddToRoleAsync(user, "Customer");

                    // Customer kaydını oluştur
                    var customerDTO = new CustomerDTO
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };

                    await _customerService.CreateCustomerAsync(customerDTO);

                    // E-posta doğrulama token'ı oluştur
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail", 
                        "Account",
                        new { userId = user.Id, token = token },
                        protocol: HttpContext.Request.Scheme);

                    // E-posta içeriği oluştur
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
                            
                            <p>Online Randevu Sistemine hoş geldiniz!</p>
                            
                            <p>E-posta adresinizi doğrulamak için aşağıdaki bağlantıya tıklayın:</p>
                            
                            <p><a href='{callbackUrl}' class='button'>E-posta Adresimi Doğrula</a></p>
                            
                            <p>E-posta doğrulamasını tamamladıktan sonra randevu oluşturabilirsiniz.</p>
                            
                            <div class='footer'>
                                <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
                            </div>
                        </div>
                    </body>
                    </html>";

                    try
                    {
                        // E-posta kuyruğuna ekle
                        var emailQueueService = HttpContext.RequestServices.GetRequiredService<EmailQueueService>();
                        emailQueueService.QueueEmail(
                            user.Email,
                            "E-posta Adresinizi Doğrulayın",
                            emailBody);

                        // Kullanıcı kaydedildi sayfasına yönlendir
                        return RedirectToAction(nameof(RegisterSuccess));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"E-posta gönderimi sırasında hata: {ex.Message}");
                        ModelState.AddModelError("", "Kayıt işlemi tamamlandı ancak doğrulama e-postası gönderilemedi. Lütfen yöneticiyle iletişime geçin.");
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterSuccess()
        {
            return View();
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"ID'si '{userId}' olan kullanıcı bulunamadı.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("ConfirmEmailSuccess");
            }

            return View("Error");
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmEmailSuccess()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            try
            {
                // Identity'nin sağladığı UserId'yi kullan
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login");
                }

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // E-posta doğrulama durumunu al
                var appUser = await _userManager.FindByIdAsync(userId);
                ViewBag.IsEmailConfirmed = appUser != null && await _userManager.IsEmailConfirmedAsync(appUser);

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profil bilgileri alınırken hata oluştu");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendEmailConfirmation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToAction("Login");
            
            // Kullanıcının e-postası zaten doğrulanmışsa
            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData["SuccessMessage"] = "E-posta adresiniz zaten doğrulanmış.";
                return RedirectToAction("Profile");
            }
            
            // E-posta doğrulama token'ı oluştur
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail", 
                "Account",
                new { userId = user.Id, token = token },
                protocol: HttpContext.Request.Scheme);

            // E-posta içeriği oluştur
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
                    
                    <p>E-posta adresinizi doğrulamak için aşağıdaki bağlantıya tıklayın:</p>
                    
                    <p><a href='{callbackUrl}' class='button'>E-posta Adresimi Doğrula</a></p>
                    
                    <p>E-posta doğrulamasını tamamladıktan sonra randevu oluşturabilirsiniz.</p>
                    
                    <div class='footer'>
                        <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
                    </div>
                </div>
            </body>
            </html>";

            try
            {
                // E-posta kuyruğuna ekle
                var emailQueueService = HttpContext.RequestServices.GetRequiredService<EmailQueueService>();
                emailQueueService.QueueEmail(
                    user.Email,
                    "E-posta Adresinizi Doğrulayın",
                    emailBody);

                TempData["SuccessMessage"] = "Doğrulama e-postası gönderildi. Lütfen e-posta kutunuzu kontrol edin.";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"E-posta gönderimi sırasında hata: {ex.Message}");
                TempData["ErrorMessage"] = "Doğrulama e-postası gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("Profile");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ProfileEdit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return RedirectToAction("Login");

            var model = new ProfileEditViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileEdit(ProfileEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return RedirectToAction("Login");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            await _userService.UpdateUserAsync(user);

            TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToAction("Login");

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("Kullanıcı şifresini başarıyla değiştirdi.");
            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
                
            var result = await _authService.ForgotPasswordAsync(model.Email);
            
            // Sonuç ne olursa olsun kullanıcıya aynı mesajı göster 
            // (güvenlik nedeniyle email'in var olup olmadığını belirtmiyoruz)
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string token)
        {
            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };
            return View(model);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
                
            var result = await _authService.ResetPasswordAsync(model.UserId, model.Token, model.Password);
            
            if (result)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            
            ModelState.AddModelError(string.Empty, "Şifre sıfırlama işlemi başarısız oldu. Lütfen tekrar deneyin.");
            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
