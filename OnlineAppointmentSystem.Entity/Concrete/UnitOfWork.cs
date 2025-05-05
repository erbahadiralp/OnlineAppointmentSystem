using System;
using System.Threading.Tasks;
using OnlineAppointmentSystem.DataAccess.Abstract;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineAppointmentSystemDbContext _context;
        private IAppointmentRepository _appointmentRepository;
        private IServiceRepository _serviceRepository;
        private IEmployeeRepository _employeeRepository;
        private ICustomerRepository _customerRepository;
        private INotificationRepository _notificationRepository;
        private IWorkingHoursRepository _workingHoursRepository;

        public UnitOfWork(OnlineAppointmentSystemDbContext context)
        {
            _context = context;
        }

        public IAppointmentRepository Appointments => _appointmentRepository ??= new AppointmentRepository(_context);
        public IServiceRepository Services => _serviceRepository ??= new ServiceRepository(_context);
        public IEmployeeRepository Employees => _employeeRepository ??= new EmployeeRepository(_context);
        public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);
        public INotificationRepository Notifications => _notificationRepository ??= new NotificationRepository(_context);
        public IWorkingHoursRepository WorkingHours => _workingHoursRepository ??= new WorkingHoursRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}