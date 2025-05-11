using OnlineAppointmentSystem.Entity.DTOs;
using System.Collections.Generic;

namespace OnlineAppointmentSystem.Web.Models.EmployeeViewModels
{
    public class EmployeeDashboardViewModel
    {
        public string EmployeeName { get; set; }
        public List<AppointmentDTO> TodayAppointments { get; set; }
        public List<AppointmentDTO> UpcomingAppointments { get; set; }
        public List<AppointmentDTO> PendingAppointments { get; set; }
    }
} 