using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAppointmentSystem.Business.Abstract;
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
                    var result = await _authService.LoginAsync(loginDTO);
                    if (result != null)
                    {
                        _logger.LogInformation($"Kullanıcı başarıyla giriş yaptı: {model.Email}");

                        // Kullanıcı bilgilerini al
                        var user = await _userManager.FindByEmailAsync(model.Email);
                        if (user != null)
                        {
                            // Önce mevcut claim'leri temizle
                            var existingClaims = await _userManager.GetClaimsAsync(user);
                            await _userManager.RemoveClaimsAsync(user, existingClaims);

                            // Yeni claim'leri ekle
                            var claims = new List<Claim>
                            {
                                new Claim("FirstName", user.FirstName ?? ""),
                                new Claim("LastName", user.LastName ?? "")
                            };
                            await _userManager.AddClaimsAsync(user, claims);

                            // Kullanıcıyı yeniden giriş yaptır
                            await _signInManager.SignOutAsync();
                            await _signInManager.SignInAsync(user, model.RememberMe);
                        }

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Geçersiz giriş denemesi: {model.Email}");
                        ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Giriş sırasında hata oluştu");
                    ModelState.AddModelError(string.Empty, "Giriş sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
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

                    // Otomatik giriş yap
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
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

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profil bilgileri alınırken hata oluştu");
                return RedirectToAction("Index", "Home");
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
    }
}
