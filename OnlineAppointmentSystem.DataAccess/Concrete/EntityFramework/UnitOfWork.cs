using OnlineAppointmentSystem.DataAccess.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using OnlineAppointmentSystem.Entity.Concrete;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineAppointmentSystemDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private IAppointmentRepository _appointmentRepository;
        private IServiceRepository _serviceRepository;
        private IEmployeeRepository _employeeRepository;
        private ICustomerRepository _customerRepository;
        private INotificationRepository _notificationRepository;
        private IWorkingHoursRepository _workingHoursRepository;
        private IUserRepository _userRepository;
        private IEmployeeServiceRepository _employeeServiceRepository;

        public UnitOfWork(OnlineAppointmentSystemDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IAppointmentRepository Appointments => _appointmentRepository ??= new AppointmentRepository(_context);
        public IServiceRepository Services => _serviceRepository ??= new ServiceRepository(_context);
        public IEmployeeRepository Employees => _employeeRepository ??= new EmployeeRepository(_context);
        public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);
        public INotificationRepository Notifications => _notificationRepository ??= new NotificationRepository(_context);
        public IWorkingHoursRepository WorkingHours => _workingHoursRepository ??= new WorkingHoursRepository(_context);
        public IUserRepository Users => _userRepository ??= new UserRepository(_userManager);
        public IEmployeeServiceRepository EmployeeServices => _employeeServiceRepository ??= new EmployeeServiceRepository(_context);

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