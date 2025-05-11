using Microsoft.Extensions.DependencyInjection;
using OnlineAppointmentSystem.Business.Abstract;
using OnlineAppointmentSystem.Business.Concrete;
using OnlineAppointmentSystem.Business.Mapping;
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
            services.AddScoped<IAppointmentService, AppointmentManager>();
            services.AddScoped<IServiceService, ServiceManager>();
            services.AddScoped<IEmployeeService, EmployeeManager>();
            services.AddScoped<ICustomerService, CustomerManager>();
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<INotificationService, NotificationManager>();
            services.AddScoped<IWorkingHoursService, WorkingHoursManager>();
            services.AddScoped<IEmailService, EmailManager>();

            return services;
        }
    }
}