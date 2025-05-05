using OnlineAppointmentSystem.Entity.DTOs;
using System.Collections.Generic;

namespace OnlineAppointmentSystem.Web.Models.AppointmentViewModels
{
    public class AppointmentListViewModel
    {
        public List<AppointmentDTO> Appointments { get; set; }
        public string SearchTerm { get; set; }
        public string StatusFilter { get; set; }
        public string DateFilter { get; set; }
    }
}