using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineAppointmentSystem.Business.Abstract;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Concrete
{
    public class EmailManager : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailManager> _logger;

        public EmailManager(IConfiguration configuration, ILogger<EmailManager> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var apiKey = _configuration["EmailSettings:SendGridApiKey"];
                _logger.LogInformation($"SendGrid API Key: {apiKey?.Substring(0, 5)}...");
                
                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogError("SendGrid API Key bulunamadı. EmailSettings:SendGridApiKey ayarı kontrol edin.");
                    return false;
                }
                
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];
                
                _logger.LogInformation($"From Email: {fromEmail}, To: {to}, Subject: {subject}");
                
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmail, fromName);
                var toEmail = new EmailAddress(to);
                var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, null, body);
                
                _logger.LogInformation("SendGrid e-postası gönderiliyor...");
                var response = await client.SendEmailAsync(msg);
                
                var statusCode = response.StatusCode;
                _logger.LogInformation($"SendGrid cevap kodu: {statusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("E-posta başarıyla gönderildi.");
                    return true;
                }
                else
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    _logger.LogError($"SendGrid Error: Status Code: {statusCode}, Response: {responseBody}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"SendGrid Email Error: {ex.Message}");
                Console.WriteLine($"SendGrid Email Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"Inner Exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
    }
}