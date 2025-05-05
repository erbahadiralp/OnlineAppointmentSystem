using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Web.Models.WorkingHoursViewModels;
using System.Collections.Generic;
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
            var employees = await _employeeService.GetActiveEmployeesAsync();
            var days = GetDaysOfWeek();

            var viewModel = new WorkingHoursViewModel
            {
                Employees = new SelectList(employees, "EmployeeId", "FullName"),
                Days = new SelectList(days, "Key", "Value")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkingHoursViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workingHoursDTO = new WorkingHoursDTO
                {
                    EmployeeId = model.EmployeeId,
                    DayOfWeek = model.DayOfWeek,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    IsActive = model.IsActive
                };

                var result = await _workingHoursService.CreateWorkingHoursAsync(workingHoursDTO);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Çalışma saati oluşturulurken bir hata oluştu.");
                }
            }

            var employees = await _employeeService.GetActiveEmployeesAsync();
            var days = GetDaysOfWeek();

            model.Employees = new SelectList(employees, "EmployeeId", "FullName");
            model.Days = new SelectList(days, "Key", "Value");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var workingHours = await _workingHoursService.GetWorkingHoursByIdAsync(id);
            if (workingHours == null)
            {
                return NotFound();
            }

            var employees = await _employeeService.GetActiveEmployeesAsync();
            var days = GetDaysOfWeek();

            var viewModel = new WorkingHoursViewModel
            {
                WorkingHoursId = workingHours.WorkingHoursId,
                EmployeeId = workingHours.EmployeeId,
                DayOfWeek = workingHours.DayOfWeek,
                StartTime = workingHours.StartTime,
                EndTime = workingHours.EndTime,
                IsActive = workingHours.IsActive,
                Employees = new SelectList(employees, "EmployeeId", "FullName"),
                Days = new SelectList(days, "Key", "Value")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WorkingHoursViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workingHoursDTO = new WorkingHoursDTO
                {
                    WorkingHoursId = model.WorkingHoursId,
                    EmployeeId = model.EmployeeId,
                    DayOfWeek = model.DayOfWeek,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    IsActive = model.IsActive
                };

                var result = await _workingHoursService.UpdateWorkingHoursAsync(workingHoursDTO);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Çalışma saati güncellenirken bir hata oluştu.");
                }
            }

            var employees = await _employeeService.GetActiveEmployeesAsync();
            var days = GetDaysOfWeek();

            model.Employees = new SelectList(employees, "EmployeeId", "FullName");
            model.Days = new SelectList(days, "Key", "Value");

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var workingHours = await _workingHoursService.GetWorkingHoursByIdAsync(id);
            if (workingHours == null)
            {
                return NotFound();
            }

            return View(workingHours);
        }

        [HttpPost, ActionName("Delete")]
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
                { 0, "Pazar" },
                { 1, "Pazartesi" },
                { 2, "Salı" },
                { 3, "Çarşamba" },
                { 4, "Perşembe" },
                { 5, "Cuma" },
                { 6, "Cumartesi" }
            };
        }
    }
}