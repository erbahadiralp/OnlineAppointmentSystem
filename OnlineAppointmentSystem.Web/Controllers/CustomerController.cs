using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Entity.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ICustomerService _customerService;

        public CustomerController(IAppointmentService appointmentService, ICustomerService customerService)
        {
            _appointmentService = appointmentService;
            _customerService = customerService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var customer = await _customerService.GetCustomerByUserIdAsync(userId);
            if (customer == null)
                return RedirectToAction("Login", "Account");

            ViewBag.CustomerName = customer.FirstName + " " + customer.LastName;

            var appointments = await _appointmentService.GetAppointmentsByCustomerIdAsync(customer.CustomerId);
            var now = DateTime.Now;
            
            // Sadece onaylanmış (Confirmed) randevuları filtrele
            var upcoming = appointments
                .Where(a => a.AppointmentDate > now && a.Status == AppointmentStatus.Confirmed)
                .OrderBy(a => a.AppointmentDate)
                .FirstOrDefault();

            return View(upcoming);
        }
    }
} 