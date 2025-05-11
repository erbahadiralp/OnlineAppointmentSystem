using System;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IAppointmentRepository Appointments { get; }
        IServiceRepository Services { get; }
        IEmployeeRepository Employees { get; }
        ICustomerRepository Customers { get; }
        INotificationRepository Notifications { get; }
        IWorkingHoursRepository WorkingHours { get; }
        IUserRepository Users { get; } // Yeni eklenen özellik
        IEmployeeServiceRepository EmployeeServices { get; } // Eğer yoksa ekleyin

        Task<int> CompleteAsync();
    }
}