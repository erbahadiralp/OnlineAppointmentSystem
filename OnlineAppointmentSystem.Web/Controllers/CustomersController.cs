using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.DTOs;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _customerService.ActivateCustomerAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _customerService.DeactivateCustomerAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
} 