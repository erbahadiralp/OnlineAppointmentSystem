using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.DataAccess.Cache;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IEmailService _emailService;
        private readonly ICacheService _cacheService;
        private readonly ICustomerService _customerService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IEmailService emailService,
            ICacheService cacheService,
            ICustomerService customerService)
        {
            _appointmentService = appointmentService;
            _emailService = emailService;
            _cacheService = cacheService;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
                return NotFound();
            return Ok(appointment);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetAppointmentsByCustomerId(int customerId)
        {
            var appointments = await _appointmentService.GetAppointmentsByCustomerIdAsync(customerId);
            return Ok(appointments);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetAppointmentsByEmployeeId(int employeeId)
        {
            var appointments = await _appointmentService.GetAppointmentsByEmployeeIdAsync(employeeId);
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentDTO appointmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _appointmentService.CreateAppointmentAsync(appointmentDto);
            if (result)
            {
                // Müşteri bilgisini çek
                var customer = await _customerService.GetCustomerByIdAsync(appointmentDto.CustomerId);
                if (customer != null)
                {
                    var mailBody = $@"
Merhaba {customer.FirstName} {customer.LastName},<br><br>
Randevunuz başarıyla oluşturuldu. Aşağıda randevu detaylarınızı bulabilirsiniz:<br><br>
<b>Tarih:</b> {appointmentDto.AppointmentDate:dd/MM/yyyy}<br>
<b>Saat:</b> {appointmentDto.AppointmentDate:HH:mm}<br>
<b>Branş:</b> {appointmentDto.ServiceName}<br>
<b>Doktor:</b> {appointmentDto.EmployeeName}<br>
";

                    await _emailService.SendEmailAsync(
                        customer.Email,
                        "Randevu Onayı",
                        mailBody
                    );
                }
                return Ok();
            }
            return BadRequest("Appointment creation failed");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentDTO appointmentDto)
        {
            if (id != appointmentDto.AppointmentId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _appointmentService.UpdateAppointmentAsync(appointmentDto);
            if (result)
                return NoContent();
            return BadRequest("Appointment update failed");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await _appointmentService.DeleteAppointmentAsync(id);
            if (result)
                return NoContent();
            return BadRequest("Appointment deletion failed");
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
                return NotFound();

            // Statüyü güncelle
            appointment.Status = OnlineAppointmentSystem.Entity.Enums.AppointmentStatus.Confirmed;
            var result = await _appointmentService.UpdateAppointmentAsync(appointment);

            if (result)
            {
                var customer = await _customerService.GetCustomerByIdAsync(appointment.CustomerId);
                if (customer != null)
                {
                    var mailBody = $@"
Merhaba {customer.FirstName} {customer.LastName},<br><br>
Randevunuz doktor tarafından onaylandı. Aşağıda randevu detaylarınızı bulabilirsiniz:<br><br>
<b>Tarih:</b> {appointment.AppointmentDate:dd/MM/yyyy}<br>
<b>Saat:</b> {appointment.AppointmentDate:HH:mm}<br>
<b>Branş:</b> {appointment.ServiceName}<br>
<b>Doktor:</b> {appointment.EmployeeName}<br>
";
                    await _emailService.SendEmailAsync(customer.Email, "Randevu Onayı", mailBody);
                }
                return Ok();
            }
            return BadRequest("Onaylama işlemi başarısız.");
        }
    }
}