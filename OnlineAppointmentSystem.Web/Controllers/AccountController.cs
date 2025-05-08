using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Web.Models.AccountViewModels;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAuthService authService,
            IUserService userService,
            ILogger<AccountController> logger)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
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
                try
                {
                    var registerDTO = new RegisterDTO
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        Password = model.Password,
                        Role = model.Role
                    };

                    var success = await _authService.RegisterAsync(registerDTO);
                    if (success)
                    {
                        _logger.LogInformation("Kullanıcı başarıyla kaydedildi.");
                        return RedirectToAction(nameof(Login));
                    }
                    else
                    {
                        _logger.LogWarning("Kullanıcı kaydı başarısız oldu.");
                        ModelState.AddModelError(string.Empty, "Kayıt işlemi başarısız oldu. Email adresi zaten kullanılıyor olabilir.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Kayıt sırasında hata oluştu");
                    ModelState.AddModelError(string.Empty, "Kayıt sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                }
            }
            else
            {
                // Model doğrulama hatalarını logla
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning($"Model doğrulama hatası: {error.ErrorMessage}");
                    }
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
                    _logger.LogWarning("Kullanıcı ID'si bulunamadı");
                    return RedirectToAction(nameof(Login));
                }

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı: {userId}");
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profil sayfası yüklenirken hata oluştu");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
