using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Web.Models.ServiceViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineAppointmentSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _serviceService.GetAllServicesAsync();
            return View(services);
        }

        public async Task<IActionResult> Details(int id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var serviceDTO = new ServiceDTO
                {
                    ServiceName = model.ServiceName,
                    Description = model.Description,
                    Price = model.Price,
                    Duration = model.Duration,
                    IsActive = model.IsActive
                };

                var result = await _serviceService.CreateServiceAsync(serviceDTO);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Hizmet oluşturulurken bir hata oluştu.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            var viewModel = new ServiceEditViewModel
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                Description = service.Description,
                Price = service.Price,
                Duration = service.Duration,
                IsActive = service.IsActive
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var serviceDTO = new ServiceDTO
                {
                    ServiceId = model.ServiceId,
                    ServiceName = model.ServiceName,
                    Description = model.Description,
                    Price = model.Price,
                    Duration = model.Duration,
                    IsActive = model.IsActive
                };

                var result = await _serviceService.UpdateServiceAsync(serviceDTO);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Hizmet güncellenirken bir hata oluştu.");
                }
            }

            return View(model);
        }

        [HttpGet]
        [Route("Services/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        [Route("Services/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _serviceService.DeleteServiceAsync(id);
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
            var result = await _serviceService.ActivateServiceAsync(id);
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
            var result = await _serviceService.DeactivateServiceAsync(id);
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