using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Entity.Enums;
using OnlineAppointmentSystem.Web.Models.AppointmentViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnlineAppointmentSystem.Entity.Concrete;
using System.Collections.Generic;

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
        private readonly IWorkingHoursService _workingHoursService;
        private readonly UserManager<AppUser> _userManager;

        public AppointmentsController(
            IAppointmentService appointmentService,
            IServiceService serviceService,
            IEmployeeService employeeService,
            ICustomerService customerService,
            IUserService userService,
            IWorkingHoursService workingHoursService,
            UserManager<AppUser> userManager)
        {
            _appointmentService = appointmentService;
            _serviceService = serviceService;
            _employeeService = employeeService;
            _customerService = customerService;
            _userService = userService;
            _workingHoursService = workingHoursService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return await Index(new AppointmentListViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Index(AppointmentListViewModel model)
        {
            // User.FindFirst("UserId") yerine ClaimTypes.NameIdentifier kullanıyoruz
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var appointments = await GetAppointmentsForUserAsync(userId);
            
            // Status filter
            if (!string.IsNullOrEmpty(model.StatusFilter) && Enum.TryParse<AppointmentStatus>(model.StatusFilter, out var status))
            {
                appointments = appointments.Where(a => a.Status == status).ToList();
            }
            
            // Date filter
            if (!string.IsNullOrEmpty(model.DateFilter) && DateTime.TryParse(model.DateFilter, out var date))
            {
                appointments = appointments.Where(a => a.AppointmentDate.Date == date.Date).ToList();
            }
            
            // Search term
            if (!string.IsNullOrEmpty(model.SearchTerm))
            {
                string searchTerm = model.SearchTerm.ToLower();
                appointments = appointments.Where(a => 
                    (a.ServiceName?.ToLower().Contains(searchTerm) == true) || 
                    (a.EmployeeName?.ToLower().Contains(searchTerm) == true) || 
                    (a.CustomerName?.ToLower().Contains(searchTerm) == true)
                ).ToList();
            }
            
            var viewModel = new AppointmentListViewModel
            {
                Appointments = appointments,
                StatusFilter = model.StatusFilter,
                DateFilter = model.DateFilter,
                SearchTerm = model.SearchTerm
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

            // Debug bilgisi
            Console.WriteLine($"Debug - Details - AppointmentId: {appointment.AppointmentId}");
            Console.WriteLine($"Debug - Details - CustomerName: {appointment.CustomerName ?? "NULL"}");
            Console.WriteLine($"Debug - Details - EmployeeName: {appointment.EmployeeName ?? "NULL"}");
            Console.WriteLine($"Debug - Details - EmployeeTitle: {appointment.EmployeeTitle ?? "NULL"}");
            Console.WriteLine($"Debug - Details - ServiceName: {appointment.ServiceName ?? "NULL"}");

            return View(appointment);
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                // User.FindFirst("UserId") yerine ClaimTypes.NameIdentifier kullanıyoruz
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var services = await _serviceService.GetActiveServicesAsync();
                var employees = await _employeeService.GetActiveEmployeesAsync();

                // Null kontrolü ve boş liste kontrolü
                if (services == null || !services.Any())
                {
                    services = new List<ServiceDTO>();
                }

                if (employees == null || !employees.Any())
                {
                    employees = new List<EmployeeDTO>();
                }

                // ServiceDTO ve EmployeeDTO null kontrolü
                services = services.Where(s => s != null && !string.IsNullOrEmpty(s.ServiceName)).ToList();
                employees = employees.Where(e => e != null && !string.IsNullOrEmpty(e.FirstName) && !string.IsNullOrEmpty(e.LastName)).ToList();

                var viewModel = new AppointmentCreateViewModel
                {
                    AppointmentDate = DateTime.Today.AddDays(1),
                    AppointmentTime = new TimeSpan(9, 0, 0),
                    Services = new SelectList(
                        services.Select(s => new { Id = s.ServiceId, Name = s.ServiceName }),
                        "Id",
                        "Name"
                    ),
                    Employees = new SelectList(
                        employees.Select(e => new { Id = e.EmployeeId, Name = $"{e.FirstName} {e.LastName}" }),
                        "Id",
                        "Name"
                    )
                };
                // Null koruması
                if (viewModel == null)
                    viewModel = new AppointmentCreateViewModel();
                viewModel.Services = viewModel.Services ?? new SelectList(new List<object>());
                viewModel.Employees = viewModel.Employees ?? new SelectList(new List<object>());
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Hata loglamayı da ekleyelim
                Console.WriteLine($"Create metodu hatası: {ex.Message}");

                // En basit viewmodel'i oluştur
                var viewModel = new AppointmentCreateViewModel
                {
                    AppointmentDate = DateTime.Today.AddDays(1),
                    AppointmentTime = new TimeSpan(9, 0, 0),
                    Services = new SelectList(new List<object>()),
                    Employees = new SelectList(new List<object>())
                };
                // Null koruması
                if (viewModel == null)
                    viewModel = new AppointmentCreateViewModel();
                viewModel.Services = viewModel.Services ?? new SelectList(new List<object>());
                viewModel.Employees = viewModel.Employees ?? new SelectList(new List<object>());
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentCreateViewModel model)
        {
            try
            {
                model.Services = model.ServiceId > 0
                    ? new SelectList(await _serviceService.GetActiveServicesAsync(), "ServiceId", "ServiceName", model.ServiceId)
                    : new SelectList(await _serviceService.GetActiveServicesAsync(), "ServiceId", "ServiceName");

                model.Employees = model.ServiceId > 0
                    ? new SelectList(
                        (await _employeeService.GetEmployeesByServiceIdAsync(model.ServiceId))
                            .Select(e => new { e.EmployeeId, FullName = e.FirstName + " " + e.LastName }),
                        "EmployeeId", "FullName")
                    : new SelectList(new List<object>());

                if (!ModelState.IsValid)
                {
                    model.Services = model.Services ?? new SelectList(new List<object>());
                    model.Employees = model.Employees ?? new SelectList(new List<object>());
                    if (model == null)
                        model = new AppointmentCreateViewModel();
                    model.Services = model.Services ?? new SelectList(new List<object>());
                    model.Employees = model.Employees ?? new SelectList(new List<object>());
                    return View(model);
                }

                if (model.AppointmentTime.Minutes % 15 != 0)
                {
                    ModelState.AddModelError("AppointmentTime", "Randevu saati sadece 15 dakikalık aralıklarla seçilebilir (örn: 09:00, 09:15, 09:30, 09:45).");
                    model.Services = model.Services ?? new SelectList(new List<object>());
                    model.Employees = model.Employees ?? new SelectList(new List<object>());
                    return View(model);
                }

                if (model.AppointmentDate == default(DateTime))
                {
                    ModelState.AddModelError("AppointmentDate", "Randevu tarihi gereklidir.");
                    model.Services = model.Services ?? new SelectList(new List<object>());
                    model.Employees = model.Employees ?? new SelectList(new List<object>());
                    return View(model);
                }
                if (model.AppointmentTime == default(TimeSpan))
                {
                    ModelState.AddModelError("AppointmentTime", "Randevu saati gereklidir.");
                    model.Services = model.Services ?? new SelectList(new List<object>());
                    model.Employees = model.Employees ?? new SelectList(new List<object>());
                    return View(model);
                }

                // User.FindFirst("UserId") yerine ClaimTypes.NameIdentifier kullanıyoruz
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }
                
                // E-posta doğrulama kontrolü
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                
                // Kullanıcı rollerini kontrol et
                var roles = await _userManager.GetRolesAsync(user);
                bool isAdminOrEmployee = roles.Contains("Admin") || roles.Contains("Employee");
                
                // Admin veya Employee değilse ve e-posta doğrulanmamışsa, randevu oluşturmaya izin verme
                if (!isAdminOrEmployee && !await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Randevu oluşturabilmek için önce e-posta adresinizi doğrulamanız gerekmektedir. Lütfen profilinizden e-posta doğrulama işlemini tamamlayın veya yeni bir doğrulama bağlantısı talep edin.");
                    model.Services = model.Services ?? new SelectList(new List<object>());
                    model.Employees = model.Employees ?? new SelectList(new List<object>());
                    
                    // E-posta doğrulama sayfasına yönlendirme bağlantısı ekle
                    ViewBag.ShowVerificationLink = true;
                    
                    return View(model);
                }

                var customer = await _customerService.GetCustomerByUserIdAsync(userId);
                if (customer == null)
                {
                    ModelState.AddModelError(string.Empty, "Müşteri bilgisi bulunamadı.");
                    model.Services = model.Services ?? new SelectList(new List<object>());
                    model.Employees = model.Employees ?? new SelectList(new List<object>());
                    return View(model);
                }

                // Yeni randevu zamanı
                var appointmentDateTime = model.AppointmentDate.Date.Add(model.AppointmentTime);
                
                // Servis süresini veritabanından al
                var service = await _serviceService.GetServiceByIdAsync(model.ServiceId);
                if (service == null)
                {
                    ModelState.AddModelError(string.Empty, "Seçilen hizmet bulunamadı.");
                    model.Services = model.Services ?? new SelectList(new List<object>());
                    model.Employees = model.Employees ?? new SelectList(new List<object>());
                    return View(model);
                }
                
                // Müşterinin aynı saatteki randevu çakışmalarını kontrol et
                var customerAppointments = await _appointmentService.GetAppointmentsByCustomerIdAsync(customer.CustomerId) ?? new List<AppointmentDTO>();
                var newAppointmentEnd = appointmentDateTime.AddMinutes(service.Duration);
                
                // Aynı saat diliminde başka bir randevu var mı kontrol et
                bool hasTimeConflict = false;
                foreach (var existingAppointment in customerAppointments)
                {
                    if (existingAppointment == null || 
                        existingAppointment.Status == OnlineAppointmentSystem.Entity.Enums.AppointmentStatus.Cancelled || 
                        existingAppointment.AppointmentDate == default(DateTime))
                        continue;
                    
                    // Mevcut randevunun hizmet süresini al
                    var existingService = await _serviceService.GetServiceByIdAsync(existingAppointment.ServiceId);
                    if (existingService == null) 
                        continue;
                    
                    var existingAppointmentStart = existingAppointment.AppointmentDate;
                    var existingAppointmentEnd = existingAppointmentStart.AddMinutes(existingService.Duration);
                    
                    // Randevu çakışmasını kontrol et
                    if (appointmentDateTime < existingAppointmentEnd && existingAppointmentStart < newAppointmentEnd)
                    {
                        hasTimeConflict = true;
                        break;
                    }
                }
                
                if (hasTimeConflict)
                {
                    ModelState.AddModelError(string.Empty, "Seçilen saatte başka bir randevunuz bulunmaktadır. Lütfen farklı bir saat seçiniz.");
                    model.Services = model.Services ?? new SelectList(new List<object>());
                    model.Employees = model.Employees ?? new SelectList(new List<object>());
                    return View(model);
                }

                var appointmentDTO = new AppointmentDTO
                {
                    CustomerId = customer.CustomerId,
                    EmployeeId = model.EmployeeId,
                    ServiceId = model.ServiceId,
                    AppointmentDate = appointmentDateTime,
                    Notes = model.Notes
                };

                var isAvailable = await _appointmentService.IsTimeSlotAvailableAsync(
                    model.EmployeeId, appointmentDateTime, service.Duration);

                if (!isAvailable)
                {
                    ModelState.AddModelError(string.Empty, "Seçilen tarih ve saatte randevu alınamaz. Lütfen başka bir zaman seçiniz.");
                    model.Services = model.Services ?? new SelectList(new List<object>());
                    model.Employees = model.Employees ?? new SelectList(new List<object>());
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
                    model.Services = model.Services ?? new SelectList(new List<object>());
                    model.Employees = model.Employees ?? new SelectList(new List<object>());
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Hata log'lama
                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
                if (model == null)
                    model = new AppointmentCreateViewModel();
                model.Services = model.Services ?? new SelectList(new List<object>());
                model.Employees = model.Employees ?? new SelectList(new List<object>());
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Employee,Customer")]
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            if (User.IsInRole("Customer"))
            {
                var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                var customer = await _customerService.GetCustomerByUserIdAsync(userId);
                if (customer == null || appointment.CustomerId != customer.CustomerId)
                    return Forbid();
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
        [Authorize(Roles = "Employee,Customer")]
        public async Task<IActionResult> Edit(AppointmentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.AppointmentTime.Minutes % 15 != 0)
                {
                    ModelState.AddModelError("AppointmentTime", "Randevu saati sadece 15 dakikalık aralıklarla seçilebilir (örn: 09:00, 09:15, 09:30, 09:45).");
                    await PrepareEditViewModelSelects(model);
                    return View(model);
                }
                var appointment = await _appointmentService.GetAppointmentByIdAsync(model.AppointmentId);
                if (appointment == null)
                    return NotFound();
                if (User.IsInRole("Customer"))
                {
                    var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                    var customer = await _customerService.GetCustomerByUserIdAsync(userId);
                    if (customer == null || appointment.CustomerId != customer.CustomerId)
                        return Forbid();
                }
                var appointmentDateTime = model.AppointmentDate.Date.Add(model.AppointmentTime);
                
                // Müşterinin zaman çakışması kontrolü (sadece müşteri ise)
                if (User.IsInRole("Customer"))
                {
                    var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                    var customer = await _customerService.GetCustomerByUserIdAsync(userId);
                    if (customer == null || appointment.CustomerId != customer.CustomerId)
                        return Forbid();
                        
                    // Servis süresini al
                    var service = await _serviceService.GetServiceByIdAsync(model.ServiceId);
                    if (service == null)
                    {
                        ModelState.AddModelError(string.Empty, "Seçilen hizmet bulunamadı.");
                        PrepareEditViewModelSelects(model);
                        return View(model);
                    }
                    
                    // Müşterinin diğer randevularını kontrol et
                    var customerAppointments = await _appointmentService.GetAppointmentsByCustomerIdAsync(customer.CustomerId) ?? new List<AppointmentDTO>();
                    var newAppointmentEnd = appointmentDateTime.AddMinutes(service.Duration);
                    
                    // Aynı saat diliminde başka bir randevu var mı kontrol et (mevcut randevu hariç)
                    bool hasTimeConflict = false;
                    foreach (var existingAppointment in customerAppointments)
                    {
                        // Kendisiyle karşılaştırmayı atla
                        if (existingAppointment.AppointmentId == model.AppointmentId)
                            continue;
                            
                        if (existingAppointment == null || 
                            existingAppointment.Status == OnlineAppointmentSystem.Entity.Enums.AppointmentStatus.Cancelled || 
                            existingAppointment.AppointmentDate == default(DateTime))
                            continue;
                        
                        // Mevcut randevunun hizmet süresini al
                        var existingService = await _serviceService.GetServiceByIdAsync(existingAppointment.ServiceId);
                        if (existingService == null) 
                            continue;
                        
                        var existingAppointmentStart = existingAppointment.AppointmentDate;
                        var existingAppointmentEnd = existingAppointmentStart.AddMinutes(existingService.Duration);
                        
                        // Randevu çakışmasını kontrol et
                        if (appointmentDateTime < existingAppointmentEnd && existingAppointmentStart < newAppointmentEnd)
                        {
                            hasTimeConflict = true;
                            break;
                        }
                    }
                    
                    if (hasTimeConflict)
                    {
                        ModelState.AddModelError(string.Empty, "Seçilen saatte başka bir randevunuz bulunmaktadır. Lütfen farklı bir saat seçiniz.");
                        PrepareEditViewModelSelects(model);
                        return View(model);
                    }
                }
                
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee,Customer")]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
                return NotFound();

            // Sadece kendi randevusunu iptal edebilsin
            if (User.IsInRole("Customer"))
            {
                var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                var customer = await _customerService.GetCustomerByUserIdAsync(userId);
                if (customer == null || appointment.CustomerId != customer.CustomerId)
                    return Forbid();
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Confirm(int id)
        {
            var result = await _appointmentService.ChangeAppointmentStatusAsync(id, AppointmentStatus.Confirmed);
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
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await _appointmentService.ChangeAppointmentStatusAsync(id, AppointmentStatus.Completed);
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
            try
            {
                if (serviceId <= 0)
                {
                    return Json(new List<object>());
                }

            var employees = await _employeeService.GetEmployeesByServiceIdAsync(serviceId);
                if (employees == null || !employees.Any())
                {
                    return Json(new List<object>());
                }

                var employeeList = employees
                    .Where(e => e != null && !string.IsNullOrEmpty(e.FirstName) && !string.IsNullOrEmpty(e.LastName))
                    .Select(e => new { id = e.EmployeeId, name = $"{e.FirstName} {e.LastName}" })
                    .ToList();

                return Json(employeeList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetEmployeesByService hatası: {ex.Message}");
                return Json(new List<object>());
            }
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

        [HttpGet]
        public async Task<IActionResult> GetAvailableTimes(int employeeId, DateTime date, int serviceId)
        {
            var service = await _serviceService.GetServiceByIdAsync(serviceId);
            if (service == null)
                return Json(new List<string>());

            var workingHours = await _workingHoursService.GetWorkingHoursByEmployeeIdAsync(employeeId);
            var dayOfWeek = (int)date.DayOfWeek;
            var dayWorkingHours = workingHours.FirstOrDefault(wh => wh.DayOfWeek == dayOfWeek && wh.IsActive);
            if (dayWorkingHours == null)
                return Json(new List<string>());

            var start = dayWorkingHours.StartTime;
            var end = dayWorkingHours.EndTime;
            var duration = service.Duration;

            var appointments = await _appointmentService.GetAppointmentsByEmployeeIdAsync(employeeId);
            var dayAppointments = appointments.Where(a => a.AppointmentDate.Date == date.Date && a.Status != AppointmentStatus.Cancelled).ToList();

            var availableTimes = new List<string>();
            for (var time = start; time.Add(TimeSpan.FromMinutes(duration)) <= end; time = time.Add(TimeSpan.FromMinutes(15)))
            {
                var slotStart = date.Date.Add(time);
                var slotEnd = slotStart.AddMinutes(duration);

                bool isBusy = dayAppointments.Any(a =>
                {
                    var aStart = a.AppointmentDate;
                    var aEnd = aStart.AddMinutes(service.Duration);
                    return slotStart < aEnd && aStart < slotEnd;
                });

                if (!isBusy)
                    availableTimes.Add(time.ToString(@"hh\:mm"));
            }

            return Json(availableTimes);
        }

        // Yardımcı metod - Edit view model için select listelerini hazırla
        private async Task PrepareEditViewModelSelects(AppointmentEditViewModel model)
        {
            var editServices = await _serviceService.GetActiveServicesAsync();
            var editEmployees = await _employeeService.GetActiveEmployeesAsync();
            var editStatuses = Enum.GetValues(typeof(AppointmentStatus))
                .Cast<AppointmentStatus>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });
            model.Services = new SelectList(editServices, "ServiceId", "ServiceName");
            model.Employees = new SelectList(editEmployees, "EmployeeId", "FullName");
            model.Statuses = new SelectList(editStatuses, "Id", "Name");
        }
    }
}
