using AutoMapper;
using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineAppointmentSystem.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Appointment mappings
            CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => $"{src.Customer.User.FirstName} {src.Customer.User.LastName}"))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => $"{src.Employee.User.FirstName} {src.Employee.User.LastName}"))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiceName))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<AppointmentDTO, Appointment>();

            // Service mappings
            CreateMap<Service, ServiceDTO>()
                .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => 
                    src.EmployeeServices.Select(es => new EmployeeDTO
                    {
                        EmployeeId = es.Employee.EmployeeId,
                        FirstName = es.Employee.User.FirstName,
                        LastName = es.Employee.User.LastName,
                        Title = es.Employee.Title,
                        Department = es.Employee.Department,
                        IsActive = es.Employee.IsActive
                    }).ToList()));

            CreateMap<ServiceDTO, Service>();

            // Employee mappings
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.ServiceIds, opt => opt.MapFrom(src => src.EmployeeServices.Select(es => es.ServiceId).ToList()))
                .ForMember(dest => dest.ServiceNames, opt => opt.MapFrom(src => src.EmployeeServices.Select(es => es.Service.ServiceName).ToList()));

            CreateMap<EmployeeDTO, Employee>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeServices, opt => opt.Ignore());

            // Customer mappings
            CreateMap<Customer, CustomerDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User.Address));

            CreateMap<CustomerDTO, Customer>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // User mappings
            CreateMap<AppUser, UserDTO>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<UserDTO, AppUser>();

            // Working hours mappings
            CreateMap<WorkingHours, WorkingHoursDTO>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => $"{src.Employee.User.FirstName} {src.Employee.User.LastName}"))
                .ForMember(dest => dest.DayName, opt => opt.MapFrom(src => GetDayName(src.DayOfWeek)));

            CreateMap<WorkingHoursDTO, WorkingHours>();

            // Notification mappings
            CreateMap<Notification, NotificationDTO>()
                .ForMember(dest => dest.AppointmentInfo, opt => opt.MapFrom(src =>
                    $"{src.Appointment.Customer.User.FirstName} {src.Appointment.Customer.User.LastName} - {src.Appointment.Service.ServiceName} - {src.Appointment.AppointmentDate}"));

            CreateMap<NotificationDTO, Notification>();
        }

        private string GetDayName(int dayOfWeek)
        {
            return dayOfWeek switch
            {
                0 => "Pazar",
                1 => "Pazartesi",
                2 => "Salı",
                3 => "Çarşamba",
                4 => "Perşembe",
                5 => "Cuma",
                6 => "Cumartesi",
                _ => "Bilinmeyen Gün"
            };
        }
    }
}