using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Web.Models.EmployeeViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Web.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;

        public EmployeeController(
            IEmployeeService employeeService,
            IAppointmentService appointmentService,
            IUserService userService)
        {
            _employeeService = employeeService;
            _appointmentService = appointmentService;
            _userService = userService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var employee = await _employeeService.GetEmployeeByUserIdAsync(userId);
            if (employee == null)
                return RedirectToAction("Login", "Account");

            var appointments = await _appointmentService.GetAppointmentsByEmployeeIdAsync(employee.EmployeeId);
            var now = DateTime.Now;

            var viewModel = new EmployeeDashboardViewModel
            {
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                TodayAppointments = appointments
                    .Where(a => a.AppointmentDate.Date == now.Date)
                    .OrderBy(a => a.AppointmentDate)
                    .ToList(),
                UpcomingAppointments = appointments
                    .Where(a => a.AppointmentDate.Date > now.Date && a.Status == Entity.Enums.AppointmentStatus.Confirmed)
                    .OrderBy(a => a.AppointmentDate)
                    .ToList(),
                PendingAppointments = appointments
                    .Where(a => a.Status == Entity.Enums.AppointmentStatus.Pending)
                    .OrderBy(a => a.AppointmentDate)
                    .ToList()
            };

            return View(viewModel);
        }
    }
} 