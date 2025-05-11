using OnlineAppointmentSystem.Entity.DTOs;
using System.Collections.Generic;

namespace OnlineAppointmentSystem.Web.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalAppointments { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalDepartments { get; set; }
        public List<AppointmentDTO> Appointments { get; set; }
        public List<EmployeeDTO> Doctors { get; set; }
        public List<ServiceDTO> Departments { get; set; }
    }
} 