using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineAppointmentSystem.Business.Abstract;
using System;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

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
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
                var useSsl = bool.Parse(_configuration["EmailSettings:UseSsl"]);
                
                _logger.LogInformation($"From Email: {fromEmail}, To: {to}, Subject: {subject}");
                
                if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
                {
                    _logger.LogError("SMTP ayarları eksik veya hatalı. Lütfen EmailSettings yapılandırmasını kontrol edin.");
                    return false;
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };

                message.Body = bodyBuilder.ToMessageBody();
                
                _logger.LogInformation("E-posta gönderiliyor...");
                
                using (var client = new SmtpClient())
                {
                    // Güvenlik sertifikasını doğrulamayı devre dışı bırak (TEST için, üretimde etkinleştirin)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    
                    // Bağlantı zaman aşımını artır
                    client.Timeout = 30000; // 30 saniye
                    
                    // SSL/TLS bağlantısı
                    await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    
                    // SMTP kimlik doğrulama
                    await client.AuthenticateAsync(smtpUsername, smtpPassword);
                    
                    // E-postayı gönder
                    await client.SendAsync(message);
                    
                    // Bağlantıyı kapat
                    await client.DisconnectAsync(true);
                }
                
                _logger.LogInformation("E-posta başarıyla gönderildi.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"E-posta Gönderim Hatası: {ex.Message}");
                Console.WriteLine($"E-posta Gönderim Hatası: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"İç Hata: {ex.InnerException.Message}");
                    Console.WriteLine($"İç Hata: {ex.InnerException.Message}");
                }
                return false;
            }
        }
    }
}