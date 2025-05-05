using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Web.Models.EmployeeViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUserService _userService;
        private readonly IServiceService _serviceService;

        public EmployeesController(
            IEmployeeService employeeService,
            IUserService userService,
            IServiceService serviceService)
        {
            _employeeService = employeeService;
            _userService = userService;
            _serviceService = serviceService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public async Task<IActionResult> Create()
        {
            var users = await _userService.GetAllUsersAsync();
            var availableUsers = users.Where(u => u.Role == "Employee" && !u.HasEmployeeProfile).ToList();

            var services = await _serviceService.GetAllServicesAsync();

            var viewModel = new EmployeeCreateViewModel
            {
                Users = new SelectList(availableUsers, "Id", "Email"),
                Services = new MultiSelectList(services, "ServiceId", "ServiceName")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employeeDTO = new EmployeeDTO
                {
                    UserId = model.UserId,
                    Title = model.Title,
                    Department = model.Department,
                    IsActive = true,
                    ServiceIds = model.ServiceIds
                };

                var result = await _employeeService.CreateEmployeeAsync(employeeDTO);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Çalışan oluşturulurken bir hata oluştu.");
                }
            }

            var users = await _userService.GetAllUsersAsync();
            var availableUsers = users.Where(u => u.Role == "Employee" && !u.HasEmployeeProfile).ToList();

            var services = await _serviceService.GetAllServicesAsync();

            model.Users = new SelectList(availableUsers, "Id", "Email");
            model.Services = new MultiSelectList(services, "ServiceId", "ServiceName");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var services = await _serviceService.GetAllServicesAsync();

            var viewModel = new EmployeeEditViewModel
            {
                EmployeeId = employee.EmployeeId,
                Title = employee.Title,
                Department = employee.Department,
                IsActive = employee.IsActive,
                ServiceIds = employee.ServiceIds,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Services = new MultiSelectList(services, "ServiceId", "ServiceName", employee.ServiceIds)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employeeDTO = new EmployeeDTO
                {
                    EmployeeId = model.EmployeeId,
                    Title = model.Title,
                    Department = model.Department,
                    IsActive = model.IsActive,
                    ServiceIds = model.ServiceIds
                };

                var result = await _employeeService.UpdateEmployeeAsync(employeeDTO);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Çalışan güncellenirken bir hata oluştu.");
                }
            }

            var services = await _serviceService.GetAllServicesAsync();
            model.Services = new MultiSelectList(services, "ServiceId", "ServiceName", model.ServiceIds);

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _employeeService.ActivateEmployeeAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _employeeService.DeactivateEmployeeAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }
    }
}