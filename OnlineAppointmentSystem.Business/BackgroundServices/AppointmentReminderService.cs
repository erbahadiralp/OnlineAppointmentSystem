using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnlineAppointmentSystem.Business.Abstract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.BackgroundServices
{
    public class AppointmentReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppointmentReminderService> _logger;

        public AppointmentReminderService(
            IServiceProvider serviceProvider,
            ILogger<AppointmentReminderService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Appointment Reminder Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Appointment Reminder Service is running at: {time}", DateTimeOffset.Now);

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
                        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                        // Randevu hatırlatmaları gönder
                        await appointmentService.SendAppointmentRemindersAsync();

                        // Bekleyen bildirimleri işle
                        await notificationService.ProcessPendingNotificationsAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing appointment reminders.");
                }

                // Her saat çalıştır
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }

            _logger.LogInformation("Appointment Reminder Service is stopping.");
        }
    }
}