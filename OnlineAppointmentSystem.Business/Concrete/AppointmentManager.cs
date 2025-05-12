using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Business.BackgroundServices;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Concrete
{
    public class AppointmentManager : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IServiceProvider _serviceProvider;

        public AppointmentManager(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            INotificationService notificationService,
            IServiceProvider serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<AppointmentDTO>> GetAllAppointmentsAsync()
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<AppointmentDTO> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
            return _mapper.Map<AppointmentDTO>(appointment);
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsByCustomerIdAsync(int customerId)
        {
            var appointments = await _unitOfWork.Appointments.GetAppointmentsByCustomerIdAsync(customerId);
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsByEmployeeIdAsync(int employeeId)
        {
            var appointments = await _unitOfWork.Appointments.GetAppointmentsByEmployeeIdAsync(employeeId);
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var appointments = await _unitOfWork.Appointments.GetAppointmentsByDateRangeAsync(startDate, endDate);
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsByStatusAsync(AppointmentStatus status)
        {
            var appointments = await _unitOfWork.Appointments.GetAppointmentsByStatusAsync(status);
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<List<AppointmentDTO>> GetUpcomingAppointmentsAsync()
        {
            var appointments = await _unitOfWork.Appointments.GetUpcomingAppointmentsAsync();
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<bool> CreateAppointmentAsync(AppointmentDTO appointmentDTO)
        {
            try
            {
                var appointment = _mapper.Map<Appointment>(appointmentDTO);
                appointment.Status = AppointmentStatus.Pending;
                appointment.CreatedDate = DateTime.Now;
                appointment.ReminderSent = false;

                await _unitOfWork.Appointments.AddAsync(appointment);
                await _unitOfWork.CompleteAsync();

                // Randevu bilgilerini eksiksiz alabilmek için tüm ilişkili verileri çekelim
                var customer = await _unitOfWork.Customers.GetByIdAsync(appointment.CustomerId);
                var user = await _unitOfWork.Users.GetUserByIdAsync(customer?.UserId ?? string.Empty);
                var employee = await _unitOfWork.Employees.GetByIdAsync(appointment.EmployeeId);
                var employeeUser = await _unitOfWork.Users.GetUserByIdAsync(employee?.UserId ?? string.Empty);
                var service = await _unitOfWork.Services.GetByIdAsync(appointment.ServiceId);

                if (customer == null || user == null || employee == null || employeeUser == null || service == null)
                    return true; // Bildirim oluşturamıyoruz ama randevu oluşturma başarılı olduğu için true dönüyoruz
                
                // E-posta içeriği
                var emailContent = $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px;'>
        <h2 style='color: #4a86e8;'>Merhaba {user.FirstName} {user.LastName},</h2>
        
        <p>Randevunuz başarıyla oluşturuldu. Aşağıda randevu detaylarınızı bulabilirsiniz:</p>
        
        <div style='background-color: #f9f9f9; padding: 15px; border-radius: 5px; margin: 20px 0;'>
            <p><strong>📅 Tarih:</strong> {appointment.AppointmentDate:dd/MM/yyyy}</p>
            <p><strong>⏰ Saat:</strong> {appointment.AppointmentDate:HH:mm}</p>
            <p><strong>🏥 Branş:</strong> {service.ServiceName}</p>
            <p><strong>👨‍⚕️ Doktor:</strong> {(string.IsNullOrEmpty(employee.Title) ? "" : employee.Title)} {employeeUser.FirstName} {employeeUser.LastName}</p>
            {(string.IsNullOrEmpty(appointment.Notes) ? "" : $"<p><strong>📝 Not:</strong> {appointment.Notes}</p>")}
        </div>
        
        <p><strong>Not:</strong> Randevunuz şu anda <span style='color: #ff9800; font-weight: bold;'>beklemede</span> durumundadır ve doktor tarafından onaylanması gerekmektedir.</p>
        <p>Randevunuz onaylandığında size tekrar bir bildirim e-postası gönderilecektir.</p>
        <p>Sorularınız için bizimle iletişime geçebilirsiniz.</p>
        <p>Sağlıklı günler dileriz.</p>
        
        <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0; font-size: 12px; color: #777;'>
            <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
        </div>
    </div>
</body>
</html>";

                // E-postayı doğrudan kuyruğa ekle
                var emailQueueService = _serviceProvider.GetRequiredService<EmailQueueService>();
                emailQueueService.QueueEmail(
                    user.Email,
                    "Randevu Oluşturuldu", 
                    emailContent);
                
                // Bildirim kaydını oluştur (isteğe bağlı)
                var notificationDTO = new NotificationDTO
                {
                    AppointmentId = appointment.AppointmentId,
                    NotificationType = "Email",
                    Content = emailContent,
                    IsSent = true, // E-posta kuyruğa eklendiği için gönderilmiş olarak işaretle
                    SentDate = DateTime.Now
                };

                await _notificationService.CreateNotificationAsync(notificationDTO);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAppointmentAsync(AppointmentDTO appointmentDTO)
        {
            try
            {
                var existingAppointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentDTO.AppointmentId);
                if (existingAppointment == null)
                    return false;

                // Tarih değiştiyse, çalışanın müsaitliğini kontrol et
                if (existingAppointment.AppointmentDate != appointmentDTO.AppointmentDate)
                {
                    var existingService = await _unitOfWork.Services.GetByIdAsync(appointmentDTO.ServiceId);
                    if (existingService == null)
                        return false;

                    var isAvailable = await IsTimeSlotAvailableAsync(appointmentDTO.EmployeeId, appointmentDTO.AppointmentDate, existingService.Duration);
                    if (!isAvailable)
                        return false;
                }

                _mapper.Map(appointmentDTO, existingAppointment);
                existingAppointment.UpdatedDate = DateTime.Now;

                _unitOfWork.Appointments.Update(existingAppointment);
                await _unitOfWork.CompleteAsync();

                // Randevu bilgilerini eksiksiz alabilmek için tüm ilişkili verileri çekelim
                var customer = await _unitOfWork.Customers.GetByIdAsync(existingAppointment.CustomerId);
                var user = await _unitOfWork.Users.GetUserByIdAsync(customer?.UserId ?? string.Empty);
                var employee = await _unitOfWork.Employees.GetByIdAsync(existingAppointment.EmployeeId);
                var employeeUser = await _unitOfWork.Users.GetUserByIdAsync(employee?.UserId ?? string.Empty);
                var service = await _unitOfWork.Services.GetByIdAsync(existingAppointment.ServiceId);

                if (customer == null || user == null || employee == null || employeeUser == null || service == null)
                    return true; // Bildirim oluşturamıyoruz ama randevu güncelleme başarılı olduğu için true dönüyoruz

                // E-posta içeriği
                var emailContent = $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px;'>
        <h2 style='color: #4a86e8;'>Merhaba {user.FirstName} {user.LastName},</h2>
        
        <p>Randevunuz güncellendi. Aşağıda güncel randevu detaylarınızı bulabilirsiniz:</p>
        
        <div style='background-color: #f9f9f9; padding: 15px; border-radius: 5px; margin: 20px 0;'>
            <p><strong>📅 Tarih:</strong> {existingAppointment.AppointmentDate:dd/MM/yyyy}</p>
            <p><strong>⏰ Saat:</strong> {existingAppointment.AppointmentDate:HH:mm}</p>
            <p><strong>🏥 Branş:</strong> {service.ServiceName}</p>
            <p><strong>👨‍⚕️ Doktor:</strong> {(string.IsNullOrEmpty(employee.Title) ? "" : employee.Title)} {employeeUser.FirstName} {employeeUser.LastName}</p>
            {(string.IsNullOrEmpty(existingAppointment.Notes) ? "" : $"<p><strong>📝 Not:</strong> {existingAppointment.Notes}</p>")}
        </div>
        
        <p>Sorularınız için bizimle iletişime geçebilirsiniz.</p>
        <p>Sağlıklı günler dileriz.</p>
        
        <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0; font-size: 12px; color: #777;'>
            <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
        </div>
    </div>
</body>
</html>";

                // E-postayı doğrudan kuyruğa ekle
                var emailQueueService = _serviceProvider.GetRequiredService<EmailQueueService>();
                emailQueueService.QueueEmail(
                    user.Email,
                    "Randevu Güncellendi", 
                    emailContent);

                // Bildirim kaydını oluştur (isteğe bağlı)
                var notificationDTO = new NotificationDTO
                {
                    AppointmentId = existingAppointment.AppointmentId,
                    NotificationType = "Email",
                    Content = emailContent,
                    IsSent = true, // E-posta kuyruğa eklendiği için gönderilmiş olarak işaretle
                    SentDate = DateTime.Now
                };

                await _notificationService.CreateNotificationAsync(notificationDTO);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            try
            {
                var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
                if (appointment == null)
                    return false;

                _unitOfWork.Appointments.Remove(appointment);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ChangeAppointmentStatusAsync(int id, AppointmentStatus status)
        {
            try
            {
                var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
                if (appointment == null)
                    return false;

                appointment.Status = status;
                appointment.UpdatedDate = DateTime.Now;

                _unitOfWork.Appointments.Update(appointment);
                await _unitOfWork.CompleteAsync();

                // Randevu bilgilerini eksiksiz alabilmek için tüm ilişkili verileri çekelim
                var customer = await _unitOfWork.Customers.GetByIdAsync(appointment.CustomerId);
                var user = await _unitOfWork.Users.GetUserByIdAsync(customer?.UserId ?? string.Empty);
                var employee = await _unitOfWork.Employees.GetByIdAsync(appointment.EmployeeId);
                var employeeUser = await _unitOfWork.Users.GetUserByIdAsync(employee?.UserId ?? string.Empty);
                var service = await _unitOfWork.Services.GetByIdAsync(appointment.ServiceId);

                if (customer == null || user == null || employee == null || employeeUser == null || service == null)
                    return true; // Bildirim oluşturamıyoruz ama randevu güncelleme başarılı olduğu için true dönüyoruz

                // Randevu durumu değiştiğinde bildirim oluştur
                var statusText = status switch
                {
                    AppointmentStatus.Confirmed => "onaylandı",
                    AppointmentStatus.Cancelled => "iptal edildi",
                    AppointmentStatus.Completed => "tamamlandı",
                    _ => "güncellendi"
                };

                // Daha ayrıntılı ve biçimlendirilmiş e-posta içeriği
                string emailContent = $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px;'>
        <h2 style='color: #4a86e8;'>Merhaba {user.FirstName} {user.LastName},</h2>
        
        <p>Randevunuz <strong>{statusText}</strong>. Aşağıda randevu detaylarınızı bulabilirsiniz:</p>
        
        <div style='background-color: #f9f9f9; padding: 15px; border-radius: 5px; margin: 20px 0;'>
            <p><strong>📅 Tarih:</strong> {appointment.AppointmentDate:dd/MM/yyyy}</p>
            <p><strong>⏰ Saat:</strong> {appointment.AppointmentDate:HH:mm}</p>
            <p><strong>🏥 Branş:</strong> {service.ServiceName}</p>
            <p><strong>👨‍⚕️ Doktor:</strong> {(string.IsNullOrEmpty(employee.Title) ? "" : employee.Title)} {employeeUser.FirstName} {employeeUser.LastName}</p>
            {(string.IsNullOrEmpty(appointment.Notes) ? "" : $"<p><strong>📝 Not:</strong> {appointment.Notes}</p>")}
        </div>
        
        <p>Sorularınız için bizimle iletişime geçebilirsiniz.</p>
        <p>Sağlıklı günler dileriz.</p>
        
        <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0; font-size: 12px; color: #777;'>
            <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
        </div>
    </div>
</body>
</html>";

                // E-postayı doğrudan kuyruğa ekle
                var emailQueueService = _serviceProvider.GetRequiredService<EmailQueueService>();
                emailQueueService.QueueEmail(
                    user.Email,
                    $"Randevu Durumu: {statusText.ToUpper()}", 
                    emailContent);

                // Bildirim kaydını oluştur (isteğe bağlı)
                var notificationDTO = new NotificationDTO
                {
                    AppointmentId = appointment.AppointmentId,
                    NotificationType = "Email",
                    Content = emailContent,
                    IsSent = true, // E-posta kuyruğa eklendiği için gönderilmiş olarak işaretle
                    SentDate = DateTime.Now
                };

                await _notificationService.CreateNotificationAsync(notificationDTO);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsTimeSlotAvailableAsync(int employeeId, DateTime appointmentDate, int duration)
        {
            try
            {
                // Çalışanın çalışma saatlerini kontrol et
                var dayOfWeek = (int)appointmentDate.DayOfWeek;
                var workingHours = await _unitOfWork.WorkingHours.GetWorkingHoursByEmployeeIdAsync(employeeId);
                var dayWorkingHours = workingHours.FirstOrDefault(wh => wh.DayOfWeek == dayOfWeek && wh.IsActive);

                if (dayWorkingHours == null)
                    return false;

                var appointmentTime = appointmentDate.TimeOfDay;
                if (appointmentTime < dayWorkingHours.StartTime || appointmentTime.Add(TimeSpan.FromMinutes(duration)) > dayWorkingHours.EndTime)
                    return false;

                // Çalışanın diğer randevularını kontrol et
                var startDate = appointmentDate.Date;
                var endDate = startDate.AddDays(1);
                var appointments = await _unitOfWork.Appointments.GetAppointmentsByDateRangeAsync(startDate, endDate);
                var employeeAppointments = appointments.Where(a => a.EmployeeId == employeeId && a.Status != AppointmentStatus.Cancelled).ToList();

                foreach (var existingAppointment in employeeAppointments)
                {
                    var existingService = await _unitOfWork.Services.GetByIdAsync(existingAppointment.ServiceId);
                    if (existingService == null) continue;

                    var existingStartTime = existingAppointment.AppointmentDate;
                    var existingEndTime = existingStartTime.AddMinutes(existingService.Duration);

                    var newStartTime = appointmentDate;
                    var newEndTime = newStartTime.AddMinutes(duration);

                    // Randevu çakışması kontrolü - sadece aynı doktorun aynı saatteki randevularını kontrol et
                    if (newStartTime < existingEndTime && existingStartTime < newEndTime)
                        return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SendAppointmentRemindersAsync()
        {
            try
            {
                var appointmentsForReminder = await _unitOfWork.Appointments.GetAppointmentsForReminderAsync();

                foreach (var appointment in appointmentsForReminder)
                {
                    // Randevu bilgilerini eksiksiz alabilmek için tüm ilişkili verileri çekelim
                    var customer = appointment.Customer;
                    var user = customer?.User;
                    var service = appointment.Service;
                    
                    if (customer == null || user == null || service == null)
                        continue; // Gerekli bilgiler yoksa bu randevu için hatırlatma göndermiyoruz
                    
                    // E-posta içeriği
                    var emailContent = $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px;'>
        <h2 style='color: #4a86e8;'>Merhaba {user.FirstName} {user.LastName},</h2>
        
        <p><strong>Randevu Hatırlatması</strong></p>
        
        <div style='background-color: #f9f9f9; padding: 15px; border-radius: 5px; margin: 20px 0;'>
            <p><strong>📅 Tarih:</strong> {appointment.AppointmentDate:dd/MM/yyyy}</p>
            <p><strong>⏰ Saat:</strong> {appointment.AppointmentDate:HH:mm}</p>
            <p><strong>🏥 Branş:</strong> {service.ServiceName}</p>
        </div>
        
        <p>Randevunuzu kaçırmamanız için bu hatırlatmayı gönderiyoruz.</p>
        <p>Sorularınız için bizimle iletişime geçebilirsiniz.</p>
        <p>Sağlıklı günler dileriz.</p>
        
        <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0; font-size: 12px; color: #777;'>
            <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
        </div>
    </div>
</body>
</html>";

                    // E-postayı doğrudan kuyruğa ekle
                    var emailQueueService = _serviceProvider.GetRequiredService<EmailQueueService>();
                    emailQueueService.QueueEmail(
                        user.Email,
                        "Randevu Hatırlatması", 
                        emailContent);

                    // Hatırlatma bildirimi oluştur (isteğe bağlı)
                    var notificationDTO = new NotificationDTO
                    {
                        AppointmentId = appointment.AppointmentId,
                        NotificationType = "Email",
                        Content = emailContent,
                        IsSent = true, // E-posta kuyruğa eklendiği için gönderilmiş olarak işaretle 
                        SentDate = DateTime.Now
                    };

                    await _notificationService.CreateNotificationAsync(notificationDTO);

                    // Randevuyu hatırlatma gönderildi olarak işaretle
                    appointment.ReminderSent = true;
                    _unitOfWork.Appointments.Update(appointment);
                }

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                // Loglama yapılabilir
            }
        }
    }
}