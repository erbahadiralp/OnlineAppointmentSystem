using AutoMapper;
using OnlineAppointmentSystem.Business.Abstract;
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

        public AppointmentManager(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
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
                // Çalışanın bu saatte müsait olup olmadığını kontrol et
                var service = await _unitOfWork.Services.GetByIdAsync(appointmentDTO.ServiceId);
                if (service == null)
                    return false;

                var isAvailable = await IsTimeSlotAvailableAsync(appointmentDTO.EmployeeId, appointmentDTO.AppointmentDate, service.Duration);
                if (!isAvailable)
                    return false;

                var appointment = _mapper.Map<Appointment>(appointmentDTO);
                appointment.CreatedDate = DateTime.Now;
                appointment.Status = AppointmentStatus.Pending;

                await _unitOfWork.Appointments.AddAsync(appointment);
                await _unitOfWork.CompleteAsync();

                // Randevu oluşturulduğunda bildirim oluştur
                var notificationDTO = new NotificationDTO
                {
                    AppointmentId = appointment.AppointmentId,
                    NotificationType = "Email",
                    Content = $"Yeni randevunuz oluşturuldu: {appointment.AppointmentDate}",
                    IsSent = false
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
                    var service = await _unitOfWork.Services.GetByIdAsync(appointmentDTO.ServiceId);
                    if (service == null)
                        return false;

                    var isAvailable = await IsTimeSlotAvailableAsync(appointmentDTO.EmployeeId, appointmentDTO.AppointmentDate, service.Duration);
                    if (!isAvailable)
                        return false;
                }

                _mapper.Map(appointmentDTO, existingAppointment);
                existingAppointment.UpdatedDate = DateTime.Now;

                _unitOfWork.Appointments.Update(existingAppointment);
                await _unitOfWork.CompleteAsync();

                // Randevu güncellendiğinde bildirim oluştur
                var notificationDTO = new NotificationDTO
                {
                    AppointmentId = existingAppointment.AppointmentId,
                    NotificationType = "Email",
                    Content = $"Randevunuz güncellendi: {existingAppointment.AppointmentDate}",
                    IsSent = false
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

                // Randevu durumu değiştiğinde bildirim oluştur
                var statusText = status switch
                {
                    AppointmentStatus.Confirmed => "onaylandı",
                    AppointmentStatus.Cancelled => "iptal edildi",
                    _ => "güncellendi"
                };

                var notificationDTO = new NotificationDTO
                {
                    AppointmentId = appointment.AppointmentId,
                    NotificationType = "Email",
                    Content = $"Randevunuz {statusText}: {appointment.AppointmentDate}",
                    IsSent = false
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
                    var existingStartTime = existingAppointment.AppointmentDate;
                    var existingEndTime = existingStartTime.AddMinutes(existingService.Duration);

                    var newStartTime = appointmentDate;
                    var newEndTime = newStartTime.AddMinutes(duration);

                    // Randevu çakışması kontrolü
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
                    // Hatırlatma bildirimi oluştur
                    var notificationDTO = new NotificationDTO
                    {
                        AppointmentId = appointment.AppointmentId,
                        NotificationType = "Email",
                        Content = $"Randevu hatırlatması: {appointment.AppointmentDate} tarihinde {appointment.Service.ServiceName} hizmetiniz bulunmaktadır.",
                        IsSent = false
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