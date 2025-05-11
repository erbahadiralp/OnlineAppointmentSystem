using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Web.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using OnlineAppointmentSystem.Entity.DTOs;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace OnlineAppointmentSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IServiceService serviceService, IUserService userService, IEmailService emailService, ILogger<HomeController> logger)
        {
            _serviceService = serviceService;
            _userService = userService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await _userService.GetUserByIdAsync(userId);
                    if (user != null)
                        ViewBag.FullName = user.FirstName + " " + user.LastName;
                }
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Dashboard", "Admin");
                else if (User.IsInRole("Employee"))
                    return RedirectToAction("Dashboard", "Employee");
                else
                    return RedirectToAction("Dashboard", "Customer");
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Service()
        {
            var services = await _serviceService.GetActiveServicesAsync();
            return View(services);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Employee()
        {
            var services = await _serviceService.GetActiveServicesAsync();
            return View(services);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult TestNotifications()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> SendTestEmail(string email)
        {
            _logger.LogInformation($"Test e-postası gönderiliyor: {email}");
            
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "E-posta adresi boş olamaz";
                return RedirectToAction("TestNotifications");
            }
            
            var result = await _emailService.SendEmailAsync(
                email, 
                "Test E-postası", 
                "<h1>Bu bir test e-postasıdır</h1><p>Online Randevu Sistemi test mesajı</p>");
                
            if (result)
            {
                TempData["Success"] = "Test e-postası başarıyla gönderildi";
            }
            else
            {
                TempData["Error"] = "Test e-postası gönderilemedi";
            }
            
            return RedirectToAction("TestNotifications");
        }
    }
}