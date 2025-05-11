using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.AppointmentViewModels
{
    public class AppointmentCreateViewModel
    {
        public AppointmentCreateViewModel()
        {
            Services = new SelectList(new List<object>());
            Employees = new SelectList(new List<object>());
            AppointmentDate = DateTime.Today;
            AppointmentTime = DateTime.Now.TimeOfDay;
        }

        [Required(ErrorMessage = "Hizmet seçimi gereklidir")]
        [Display(Name = "Hizmet")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Çalışan seçimi gereklidir")]
        [Display(Name = "Çalışan")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Randevu tarihi gereklidir")]
        [Display(Name = "Randevu Tarihi")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Randevu saati gereklidir")]
        [Display(Name = "Randevu Saati")]
        [DataType(DataType.Time)]
        public TimeSpan AppointmentTime { get; set; }

        [Display(Name = "Not")]
        public string? Notes { get; set; }

        // Dropdown listeleri için
        public SelectList Services { get; set; }
        public SelectList Employees { get; set; }
    }
}