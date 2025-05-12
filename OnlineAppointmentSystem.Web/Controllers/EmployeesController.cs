using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Web.Models.EmployeeViewModels;
using System;
using System.Collections.Generic;
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
        private readonly UserManager<AppUser> _userManager;

        public EmployeesController(
            IEmployeeService employeeService,
            IUserService userService,
            IServiceService serviceService,
            UserManager<AppUser> userManager)
        {
            _employeeService = employeeService;
            _userService = userService;
            _serviceService = serviceService;
            _userManager = userManager;
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
            var services = await _serviceService.GetAllServicesAsync();
            var viewModel = new EmployeeCreateViewModel
            {
                Services = new SelectList(services, "ServiceId", "ServiceName")
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            Console.WriteLine($"[LOG] Create POST - Role: {model.Role}, ServiceId: {model.ServiceId}, Email: {model.Email}");
            if (model.Role == "Employee")
            {
                if (!model.ServiceId.HasValue)
                    ModelState.AddModelError("ServiceId", "Bir hizmet seçmelisiniz.");
                if (string.IsNullOrWhiteSpace(model.Title))
                    ModelState.AddModelError("Title", "Unvan alanı zorunludur.");
                if (string.IsNullOrWhiteSpace(model.Department))
                    ModelState.AddModelError("Department", "Departman alanı zorunludur.");
            }
            else if (model.Role == "Admin")
            {
                ModelState.Remove("Title");
                ModelState.Remove("Department");
                ModelState.Remove("ServiceId");
            }
            ModelState.Remove("UserId");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("[LOG] ModelState is invalid. Errors:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"- {error.ErrorMessage}");
                }
                var services = await _serviceService.GetAllServicesAsync();
                model.Services = new SelectList(services, "ServiceId", "ServiceName");
                return View(model);
            }

            try
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = FormatPhoneNumber(model.PhoneNumber),
                    Address = model.Address,
                    CreatedDate = DateTime.Now
                };
                Console.WriteLine($"[LOG] Creating user: {user.Email}");
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    Console.WriteLine("[LOG] User created successfully. Assigning role...");
                    await _userManager.AddToRoleAsync(user, model.Role);

                    if (model.Role == "Employee")
                    {
                        var employeeDTO = new EmployeeDTO
                        {
                            UserId = user.Id,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,
                            Title = model.Title,
                            Department = model.Department,
                            IsActive = true,
                            ServiceIds = model.ServiceId.HasValue ? new List<int> { model.ServiceId.Value } : new List<int>()
                        };
                        Console.WriteLine($"[LOG] Creating employee record for userId: {user.Id}, ServiceId: {model.ServiceId}");
                        var employeeResult = await _employeeService.CreateEmployeeAsync(employeeDTO);
                        Console.WriteLine($"[LOG] Employee creation result: {employeeResult}");
                        if (employeeResult)
                        {
                            TempData["SuccessMessage"] = "Doktor başarıyla oluşturuldu.";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            Console.WriteLine("[LOG] Employee creation failed. Deleting user...");
                            await _userManager.DeleteAsync(user);
                            ModelState.AddModelError(string.Empty, "Doktor oluşturulurken bir hata oluştu.");
                        }
                    }
                    else if (model.Role == "Admin")
                    {
                        TempData["SuccessMessage"] = "Admin başarıyla oluşturuldu.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    Console.WriteLine("[LOG] User creation failed. Errors:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOG] Exception: {ex.Message}");
                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
            }

            var allServices = await _serviceService.GetAllServicesAsync();
            model.Services = new SelectList(allServices, "ServiceId", "ServiceName");
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Console.WriteLine($"[LOG] Edit GET - id: {id}");
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                Console.WriteLine("[LOG] Edit GET - Employee not found.");
                return NotFound();
            }

            var services = await _serviceService.GetAllServicesAsync();
            var viewModel = new EmployeeEditViewModel
            {
                EmployeeId = employee.EmployeeId,
                UserId = employee.UserId,
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
            Console.WriteLine($"[LOG] Edit GET - Loaded employee: {employee.FirstName} {employee.LastName}");
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEditViewModel model)
        {
            Console.WriteLine($"[LOG] Edit POST - EmployeeId: {model.EmployeeId}, Name: {model.FirstName} {model.LastName}, ServiceIds: {string.Join(",", model.ServiceIds ?? new List<int>())}");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("[LOG] Edit POST - ModelState is invalid. Errors:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"- {error.ErrorMessage}");
                }
                var services = await _serviceService.GetAllServicesAsync();
                model.Services = new MultiSelectList(services, "ServiceId", "ServiceName", model.ServiceIds);
                return View(model);
            }

            var employeeDTO = new EmployeeDTO
            {
                EmployeeId = model.EmployeeId,
                UserId = model.UserId,
                Title = model.Title,
                Department = model.Department,
                IsActive = model.IsActive,
                ServiceIds = model.ServiceIds
            };
            Console.WriteLine($"[LOG] Edit POST - Updating employee: {model.EmployeeId}");
            var result = await _employeeService.UpdateEmployeeAsync(employeeDTO);
            Console.WriteLine($"[LOG] Edit POST - Update result: {result}");
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("[LOG] Edit POST - Update failed.");
                ModelState.AddModelError(string.Empty, "Çalışan güncellenirken bir hata oluştu.");
            }

            var allServices = await _serviceService.GetAllServicesAsync();
            model.Services = new MultiSelectList(allServices, "ServiceId", "ServiceName", model.ServiceIds);
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
    }
}