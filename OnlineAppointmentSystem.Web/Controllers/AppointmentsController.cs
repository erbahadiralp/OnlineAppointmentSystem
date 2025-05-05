using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Entity.Enums;
using OnlineAppointmentSystem.Web.Models.AppointmentViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Web.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceService _serviceService;
        private readonly IEmployeeService _employeeService;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;

        public AppointmentsController(
            IAppointmentService appointmentService,
            IServiceService serviceService,
            IEmployeeService employeeService,
            ICustomerService customerService,
            IUserService userService)
        {
            _appointmentService = appointmentService;
            _serviceService = serviceService;
            _employeeService = employeeService;
            _customerService = customerService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var appointments = await GetAppointmentsForUserAsync(userId);
            var viewModel = new AppointmentListViewModel
            {
                Appointments = appointments
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var services = await _serviceService.GetActiveServicesAsync();
            var employees = await _employeeService.GetActiveEmployeesAsync();

            var viewModel = new AppointmentCreateViewModel
            {
                AppointmentDate = DateTime.Today.AddDays(1),
                AppointmentTime = new TimeSpan(9, 0, 0),
                Services = new SelectList(services, "ServiceId", "ServiceName"),
                Employees = new SelectList(employees, "EmployeeId", "FullName")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var customer = await _customerService.GetCustomerByUserIdAsync(userId);
                if (customer == null)
                {
                    ModelState.AddModelError(string.Empty, "Müşteri bilgisi bulunamadı.");
                    return View(model);
                }

                var appointmentDateTime = model.AppointmentDate.Date.Add(model.AppointmentTime);
                var appointmentDTO = new AppointmentDTO
                {
                    CustomerId = customer.CustomerId,
                    EmployeeId = model.EmployeeId,
                    ServiceId = model.ServiceId,
                    AppointmentDate = appointmentDateTime,
                    Notes = model.Notes
                };

                var isAvailable = await _appointmentService.IsTimeSlotAvailableAsync(
                    model.EmployeeId, appointmentDateTime, 60); // Süreyi servis bilgisinden almalıyız

                if (!isAvailable)
                {
                    ModelState.AddModelError(string.Empty, "Seçilen tarih ve saatte randevu alınamaz. Lütfen başka bir zaman seçiniz.");

                    var services = await _serviceService.GetActiveServicesAsync();
                    var employees = await _employeeService.GetActiveEmployeesAsync();

                    model.Services = new SelectList(services, "ServiceId", "ServiceName");
                    model.Employees = new SelectList(employees, "EmployeeId", "FullName");

                    return View(model);
                }

                var result = await _appointmentService.CreateAppointmentAsync(appointmentDTO);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Randevu oluşturulurken bir hata oluştu.");
                }
            }

            var servicesForError = await _serviceService.GetActiveServicesAsync();
            var employeesForError = await _employeeService.GetActiveEmployeesAsync();

            model.Services = new SelectList(servicesForError, "ServiceId", "ServiceName");
            model.Employees = new SelectList(employeesForError, "EmployeeId", "FullName");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var services = await _serviceService.GetActiveServicesAsync();
            var employees = await _employeeService.GetActiveEmployeesAsync();
            var statuses = Enum.GetValues(typeof(AppointmentStatus))
                .Cast<AppointmentStatus>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });

            var viewModel = new AppointmentEditViewModel
            {
                AppointmentId = appointment.AppointmentId,
                ServiceId = appointment.ServiceId,
                EmployeeId = appointment.EmployeeId,
                AppointmentDate = appointment.AppointmentDate.Date,
                AppointmentTime = appointment.AppointmentDate.TimeOfDay,
                Notes = appointment.Notes,
                Status = appointment.Status,
                Services = new SelectList(services, "ServiceId", "ServiceName"),
                Employees = new SelectList(employees, "EmployeeId", "FullName"),
                Statuses = new SelectList(statuses, "Id", "Name")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppointmentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointmentDateTime = model.AppointmentDate.Date.Add(model.AppointmentTime);
                var appointmentDTO = new AppointmentDTO
                {
                    AppointmentId = model.AppointmentId,
                    ServiceId = model.ServiceId,
                    EmployeeId = model.EmployeeId,
                    AppointmentDate = appointmentDateTime,
                    Notes = model.Notes,
                    Status = model.Status
                };

                var result = await _appointmentService.UpdateAppointmentAsync(appointmentDTO);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Randevu güncellenirken bir hata oluştu.");
                }
            }

            var services = await _serviceService.GetActiveServicesAsync();
            var employees = await _employeeService.GetActiveEmployeesAsync();
            var statuses = Enum.GetValues(typeof(AppointmentStatus))
                .Cast<AppointmentStatus>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });

            model.Services = new SelectList(services, "ServiceId", "ServiceName");
            model.Employees = new SelectList(employees, "EmployeeId", "FullName");
            model.Statuses = new SelectList(statuses, "Id", "Name");

            return View(model);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var result = await _appointmentService.ChangeAppointmentStatusAsync(id, AppointmentStatus.Cancelled);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        private async Task<System.Collections.Generic.List<AppointmentDTO>> GetAppointmentsForUserAsync(string userId)
        {
            if (User.IsInRole("Admin"))
            {
                return await _appointmentService.GetAllAppointmentsAsync();
            }
            else if (User.IsInRole("Employee"))
            {
                var employee = await _employeeService.GetEmployeeByUserIdAsync(userId);
                if (employee != null)
                {
                    return await _appointmentService.GetAppointmentsByEmployeeIdAsync(employee.EmployeeId);
                }
            }
            else // Customer
            {
                var customer = await _customerService.GetCustomerByUserIdAsync(userId);
                if (customer != null)
                {
                    return await _appointmentService.GetAppointmentsByCustomerIdAsync(customer.CustomerId);
                }
            }

            return new System.Collections.Generic.List<AppointmentDTO>();
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesByService(int serviceId)
        {
            var employees = await _employeeService.GetEmployeesByServiceIdAsync(serviceId);
            return Json(employees.Select(e => new { id = e.EmployeeId, name = $"{e.FirstName} {e.LastName}" }));
        }

        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int employeeId, DateTime date, TimeSpan time, int serviceId)
        {
            var service = await _serviceService.GetServiceByIdAsync(serviceId);
            if (service == null)
            {
                return Json(new { available = false, message = "Hizmet bulunamadı." });
            }

            var appointmentDateTime = date.Date.Add(time);
            var isAvailable = await _appointmentService.IsTimeSlotAvailableAsync(
                employeeId, appointmentDateTime, service.Duration);

            return Json(new { available = isAvailable });
        }
    }
}