using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Abstract
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}