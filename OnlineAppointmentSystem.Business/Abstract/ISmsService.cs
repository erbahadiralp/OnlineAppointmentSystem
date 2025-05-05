using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Abstract
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(string phoneNumber, string message);
    }
}