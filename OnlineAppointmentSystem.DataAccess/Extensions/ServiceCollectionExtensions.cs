using OnlineAppointmentSystem.DataAccess.Abstract;
using OnlineAppointmentSystem.DataAccess.Cache;
using OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace OnlineAppointmentSystem.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext - Program.cs'de zaten yapılandırıldığı için kaldırabilirsiniz
            // veya burada bırakıp Program.cs'den kaldırabilirsiniz
            services.AddDbContext<OnlineAppointmentSystemDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Identity yapılandırmasını kaldırıyoruz - Bu Program.cs'de yapılmalı
            // services.AddIdentity<AppUser, IdentityRole>... - BU KISMI KALDIRIN

            // Repositories
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IWorkingHoursRepository, WorkingHoursRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Cache
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, MemoryCacheService>();

            // Redis Cache (opsiyonel)
            var useRedisCache = configuration.GetValue<bool>("UseRedisCache");
            if (useRedisCache)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = configuration.GetConnectionString("RedisConnection");
                    options.InstanceName = "HospitalAppointment:";
                });
                services.AddSingleton<ICacheService, RedisCacheService>();
            }

            return services;
        }
    }
}