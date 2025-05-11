using OnlineAppointmentSystem.Entity.Concrete;
using OnlineAppointmentSystem.Entity.DTOs;
using OnlineAppointmentSystem.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.Business.Abstract
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDTO>> GetAllAppointmentsAsync();
        Task<AppointmentDTO> GetAppointmentByIdAsync(int id);
        Task<List<AppointmentDTO>> GetAppointmentsByCustomerIdAsync(int customerId);
        Task<List<AppointmentDTO>> GetAppointmentsByEmployeeIdAsync(int employeeId);
        Task<List<AppointmentDTO>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<AppointmentDTO>> GetAppointmentsByStatusAsync(AppointmentStatus status);
        Task<List<AppointmentDTO>> GetUpcomingAppointmentsAsync();
        Task<bool> CreateAppointmentAsync(AppointmentDTO appointmentDTO);
        Task<bool> UpdateAppointmentAsync(AppointmentDTO appointmentDTO);
        Task<bool> DeleteAppointmentAsync(int id);
        Task<bool> ChangeAppointmentStatusAsync(int id, AppointmentStatus status);
        Task<bool> IsTimeSlotAvailableAsync(int employeeId, DateTime appointmentDate, int duration);
        Task SendAppointmentRemindersAsync();
    }
}