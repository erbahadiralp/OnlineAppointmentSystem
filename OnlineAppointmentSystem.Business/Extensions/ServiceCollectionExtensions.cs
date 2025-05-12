using Microsoft.Extensions.DependencyInjection;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Business.Concrete;
using OnlineAppointmentSystem.Business.Mapping;
using OnlineAppointmentSystem.DataAccess.Abstract;
using AutoMapper;
using System;

namespace OnlineAppointmentSystem.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Services
            services.AddScoped<IAppointmentService>(provider => new AppointmentManager(
                provider.GetRequiredService<IUnitOfWork>(),
                provider.GetRequiredService<IMapper>(),
                provider.GetRequiredService<INotificationService>(),
                provider));
            services.AddScoped<IServiceService, ServiceManager>();
            services.AddScoped<IEmployeeService, EmployeeManager>();
            services.AddScoped<ICustomerService, CustomerManager>();
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<INotificationService>(provider => new NotificationManager(
                provider.GetRequiredService<IUnitOfWork>(),
                provider.GetRequiredService<IMapper>(),
                provider.GetRequiredService<IEmailService>(),
                provider));
            services.AddScoped<IWorkingHoursService, WorkingHoursManager>();
            services.AddScoped<IEmailService, EmailManager>();

            return services;
        }
    }
}