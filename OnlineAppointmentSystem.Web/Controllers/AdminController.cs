using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Web.Models;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IEmployeeService _employeeService;
        private readonly IServiceService _serviceService;

        public AdminController(
            IAppointmentService appointmentService,
            IEmployeeService employeeService,
            IServiceService serviceService)
        {
            _appointmentService = appointmentService;
            _employeeService = employeeService;
            _serviceService = serviceService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            var doctors = await _employeeService.GetAllEmployeesAsync();
            var departments = await _serviceService.GetAllServicesAsync();

            var viewModel = new AdminDashboardViewModel
            {
                TotalAppointments = appointments.Count,
                TotalDoctors = doctors.Count,
                TotalDepartments = departments.Count,
                Appointments = appointments,
                Doctors = doctors,
                Departments = departments
            };

            return View(viewModel);
        }
    }
} 