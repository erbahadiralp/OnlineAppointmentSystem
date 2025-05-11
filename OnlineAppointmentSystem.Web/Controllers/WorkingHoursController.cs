using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Web.Models.WorkingHoursViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WorkingHoursController : Controller
    {
        private readonly IWorkingHoursService _workingHoursService;
        private readonly IEmployeeService _employeeService;

        public WorkingHoursController(
            IWorkingHoursService workingHoursService,
            IEmployeeService employeeService)
        {
            _workingHoursService = workingHoursService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var workingHours = await _workingHoursService.GetAllWorkingHoursAsync();
            return View(workingHours);
        }

        public async Task<IActionResult> Details(int id)
        {
            var workingHours = await _workingHoursService.GetWorkingHoursByIdAsync(id);
            if (workingHours == null)
            {
                return NotFound();
            }

            return View(workingHours);
        }

        public async Task<IActionResult> Create()
        {
            try
        {
            var employees = await _employeeService.GetActiveEmployeesAsync();
                if (employees == null)
                {
                    employees = new List<EmployeeDTO>();
                }

                // Employee verilerini filtrele ve dönüştür
                var employeeList = employees
                    .Where(e => e != null && !string.IsNullOrEmpty(e.FirstName) && !string.IsNullOrEmpty(e.LastName))
                    .Select(e => new { Id = e.EmployeeId, Name = $"{e.FirstName} {e.LastName}" })
                    .ToList();

            var days = GetDaysOfWeek();
                if (days == null || !days.Any())
                {
                    days = new Dictionary<int, string>
                    {
                        { 0, "Pazartesi" },
                        { 1, "Salı" },
                        { 2, "Çarşamba" },
                        { 3, "Perşembe" },
                        { 4, "Cuma" },
                        { 5, "Cumartesi" },
                        { 6, "Pazar" }
                    };
                }

            var viewModel = new WorkingHoursViewModel
            {
                    Employees = new SelectList(employeeList, "Id", "Name"),
                    Days = new SelectList(days, "Key", "Value"),
                    IsActive = true,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0)
            };

            return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WorkingHours Create action error: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkingHoursViewModel model)
        {
            try
            {
                Console.WriteLine("Create action started");

                // Dropdown listeleri yükle
                var employees = await _employeeService.GetActiveEmployeesAsync() ?? new List<EmployeeDTO>();
                var employeeList = employees
                    .Where(e => e != null && !string.IsNullOrEmpty(e.FirstName) && !string.IsNullOrEmpty(e.LastName))
                    .Select(e => new { Id = e.EmployeeId, Name = $"{e.FirstName} {e.LastName}" })
                    .ToList();

                var days = GetDaysOfWeek();
                if (days == null || !days.Any())
                {
                    days = new Dictionary<int, string>
                    {
                        { 0, "Pazartesi" },
                        { 1, "Salı" },
                        { 2, "Çarşamba" },
                        { 3, "Perşembe" },
                        { 4, "Cuma" },
                        { 5, "Cumartesi" },
                        { 6, "Pazar" }
                    };
                }

                model.Employees = new SelectList(employeeList, "Id", "Name");
                model.Days = new SelectList(days, "Key", "Value");

                // ModelState'den Days ve Employees hatalarını temizle
                ModelState.Remove("Days");
                ModelState.Remove("Employees");

                Console.WriteLine($"Model state is valid: {ModelState.IsValid}");

                if (!ModelState.IsValid)
            {
                    foreach (var key in ModelState.Keys)
                    {
                        var errors = ModelState[key].Errors;
                        foreach (var error in errors)
                        {
                            Console.WriteLine($"ModelState Error - {key}: {error.ErrorMessage}");
                        }
                    }
                    return View(model);
                }

                bool success = true;
                foreach (var day in model.SelectedDays)
                {
                    var workingHoursDTO = new WorkingHoursDTO
                    {
                        EmployeeId = model.EmployeeId,
                        DayOfWeek = day,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        IsActive = model.IsActive
                    };

                    var result = await _workingHoursService.CreateWorkingHoursAsync(workingHoursDTO);
                    if (!result)
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    Console.WriteLine("Successfully created working hours, redirecting to Index");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Console.WriteLine("Failed to create working hours");
                    ModelState.AddModelError(string.Empty, "Çalışma saati oluşturulurken bir hata oluştu.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create action: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }
                ModelState.AddModelError(string.Empty, "Bir hata oluştu. Lütfen tekrar deneyiniz.");
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
        {
            var workingHours = await _workingHoursService.GetWorkingHoursByIdAsync(id);
            if (workingHours == null)
            {
                return NotFound();
            }

                var employees = await _employeeService.GetActiveEmployeesAsync() ?? new List<EmployeeDTO>();
                var employeeList = employees
                    .Where(e => e != null && !string.IsNullOrEmpty(e.FirstName) && !string.IsNullOrEmpty(e.LastName))
                    .Select(e => new { Id = e.EmployeeId, Name = $"{e.FirstName} {e.LastName}" })
                    .ToList();

            var days = GetDaysOfWeek();
                if (days == null || !days.Any())
                {
                    days = new Dictionary<int, string>
                    {
                        { 0, "Pazartesi" },
                        { 1, "Salı" },
                        { 2, "Çarşamba" },
                        { 3, "Perşembe" },
                        { 4, "Cuma" },
                        { 5, "Cumartesi" },
                        { 6, "Pazar" }
                    };
                }

            var viewModel = new WorkingHoursViewModel
            {
                WorkingHoursId = workingHours.WorkingHoursId,
                EmployeeId = workingHours.EmployeeId,
                SelectedDays = new List<int> { workingHours.DayOfWeek },
                StartTime = workingHours.StartTime,
                EndTime = workingHours.EndTime,
                IsActive = workingHours.IsActive,
                Employees = new SelectList(employeeList, "Id", "Name"),
                Days = new SelectList(days, "Key", "Value")
            };

            return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WorkingHours Edit action error: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WorkingHoursViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Önce mevcut çalışma saatlerini sil
                    var existingHours = await _workingHoursService.GetWorkingHoursByEmployeeIdAsync(model.EmployeeId);
                    foreach (var hour in existingHours)
                    {
                        await _workingHoursService.DeleteWorkingHoursAsync(hour.WorkingHoursId);
                    }

                    // Yeni çalışma saatlerini ekle
                    bool success = true;
                    foreach (var day in model.SelectedDays)
                    {
                        var workingHoursDTO = new WorkingHoursDTO
                        {
                            EmployeeId = model.EmployeeId,
                            DayOfWeek = day,
                            StartTime = model.StartTime,
                            EndTime = model.EndTime,
                            IsActive = model.IsActive
                        };

                        var result = await _workingHoursService.CreateWorkingHoursAsync(workingHoursDTO);
                        if (!result)
                        {
                            success = false;
                            break;
                        }
                    }

                    if (success)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Çalışma saati güncellenirken bir hata oluştu.");
                    }
                }

                var employees = await _employeeService.GetActiveEmployeesAsync() ?? new List<EmployeeDTO>();
                var employeeList = employees
                    .Where(e => e != null && !string.IsNullOrEmpty(e.FirstName) && !string.IsNullOrEmpty(e.LastName))
                    .Select(e => new { Id = e.EmployeeId, Name = $"{e.FirstName} {e.LastName}" })
                    .ToList();

                var days = GetDaysOfWeek();
                if (days == null || !days.Any())
                {
                    days = new Dictionary<int, string>
                    {
                        { 0, "Pazartesi" },
                        { 1, "Salı" },
                        { 2, "Çarşamba" },
                        { 3, "Perşembe" },
                        { 4, "Cuma" },
                        { 5, "Cumartesi" },
                        { 6, "Pazar" }
                    };
                }

                model.Employees = new SelectList(employeeList, "Id", "Name");
                model.Days = new SelectList(days, "Key", "Value");

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WorkingHours Edit POST action error: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Bir hata oluştu. Lütfen tekrar deneyiniz.");
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("WorkingHours/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var workingHours = await _workingHoursService.GetWorkingHoursByIdAsync(id);
            if (workingHours == null)
            {
                return NotFound();
            }

            return View(workingHours);
        }

        [HttpPost]
        [Route("WorkingHours/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _workingHoursService.DeleteWorkingHoursAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        private Dictionary<int, string> GetDaysOfWeek()
        {
            return new Dictionary<int, string>
            {
                { 1, "Pazartesi" },
                { 2, "Salı" },
                { 3, "Çarşamba" },
                { 4, "Perşembe" },
                { 5, "Cuma" },
                { 6, "Cumartesi" },
                { 0, "Pazar" }
            };
        }
    }
}