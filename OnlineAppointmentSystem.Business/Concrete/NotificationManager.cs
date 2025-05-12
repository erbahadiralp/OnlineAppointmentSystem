using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Business.BackgroundServices;
using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineAppointmentSystem.Business.Concrete
{
    public class NotificationManager : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IServiceProvider _serviceProvider;

        public NotificationManager(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmailService emailService,
            IServiceProvider serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<NotificationDTO>> GetAllNotificationsAsync()
        {
            var notifications = await _unitOfWork.Notifications.GetAllAsync();
            return _mapper.Map<List<NotificationDTO>>(notifications);
        }

        public async Task<NotificationDTO> GetNotificationByIdAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            return _mapper.Map<NotificationDTO>(notification);
        }

        public async Task<List<NotificationDTO>> GetNotificationsByAppointmentIdAsync(int appointmentId)
        {
            var notifications = await _unitOfWork.Notifications.FindAsync(n => n.AppointmentId == appointmentId);
            return _mapper.Map<List<NotificationDTO>>(notifications);
        }

        public async Task<List<NotificationDTO>> GetPendingNotificationsAsync()
        {
            var notifications = await _unitOfWork.Notifications.FindAsync(n => !n.IsSent);
            return _mapper.Map<List<NotificationDTO>>(notifications);
        }

        public async Task<bool> CreateNotificationAsync(NotificationDTO notificationDTO)
        {
            try
            {
                var notification = _mapper.Map<Notification>(notificationDTO);
                notification.CreatedDate = DateTime.Now;
                notification.IsSent = false;

                await _unitOfWork.Notifications.AddAsync(notification);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateNotificationAsync(NotificationDTO notificationDTO)
        {
            try
            {
                var existingNotification = await _unitOfWork.Notifications.GetByIdAsync(notificationDTO.NotificationId);
                if (existingNotification == null)
                    return false;

                _mapper.Map(notificationDTO, existingNotification);

                _unitOfWork.Notifications.Update(existingNotification);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            try
            {
                var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
                if (notification == null)
                    return false;

                _unitOfWork.Notifications.Remove(notification);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendEmailNotificationAsync(NotificationDTO notificationDTO)
        {
            try
            {
                // Bu işlemi repository'de özel bir metot olarak uygulamanız gerekebilir
                var appointment = await _unitOfWork.Appointments.GetByIdAsync(notificationDTO.AppointmentId);
                if (appointment == null)
                    return false;

                var customer = await _unitOfWork.Customers.GetByIdAsync(appointment.CustomerId);
                if (customer == null)
                    return false;

                var user = await _unitOfWork.Users.GetUserByIdAsync(customer.UserId);
                if (user == null)
                    return false;

                // EmailQueueService'i kullanarak e-postayı kuyruğa ekle
                var emailQueueService = _serviceProvider.GetRequiredService<EmailQueueService>();
                emailQueueService.QueueEmail(
                    user.Email,
                    "Randevu Bilgilendirmesi",
                    notificationDTO.Content);

                // Bildirimi gönderilmiş olarak işaretle
                var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationDTO.NotificationId);
                notification.IsSent = true;
                notification.SentDate = DateTime.Now;

                _unitOfWork.Notifications.Update(notification);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task ProcessPendingNotificationsAsync()
        {
            try
            {
                var pendingNotifications = await _unitOfWork.Notifications.FindAsync(n => !n.IsSent);

                foreach (var notification in pendingNotifications)
                {
                    bool sent = false;
                    var notificationDTO = _mapper.Map<NotificationDTO>(notification);

                    if (notification.NotificationType.Equals("Email", StringComparison.OrdinalIgnoreCase))
                    {
                        sent = await SendEmailNotificationAsync(notificationDTO);
                    }

                    if (sent)
                    {
                        notification.IsSent = true;
                        notification.SentDate = DateTime.Now;
                        _unitOfWork.Notifications.Update(notification);
                    }
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