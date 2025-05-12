using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnlineAppointmentSystem.Business.Abstract;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.BackgroundServices
{
    public class EmailQueueService : BackgroundService
    {
        private readonly ConcurrentQueue<EmailMessage> _emailQueue = new ConcurrentQueue<EmailMessage>();
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailQueueService> _logger;
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public EmailQueueService(
            IServiceProvider serviceProvider,
            ILogger<EmailQueueService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void QueueEmail(string to, string subject, string body)
        {
            _emailQueue.Enqueue(new EmailMessage { To = to, Subject = subject, Body = body });
            _signal.Release(); // Sinyal ver, yeni e-posta var
            _logger.LogInformation($"Email added to queue. To: {to}, Subject: {subject}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email Queue Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                // Sinyali bekle, yeni e-posta geldiğinde devam et
                await _signal.WaitAsync(stoppingToken);

                try
                {
                    // Kuyruktaki tüm e-postaları işle
                    while (_emailQueue.TryDequeue(out var email))
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            try
                            {
                                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                                var result = await emailService.SendEmailAsync(email.To, email.Subject, email.Body);
                                
                                if (result)
                                {
                                    _logger.LogInformation($"Email sent successfully. To: {email.To}, Subject: {email.Subject}");
                                }
                                else
                                {
                                    _logger.LogWarning($"Failed to send email. To: {email.To}, Subject: {email.Subject}");
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Error sending email to {email.To}: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in the email queue service.");
                }
            }

            _logger.LogInformation("Email Queue Service is stopping.");
        }
    }

    public class EmailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
} 