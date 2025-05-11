using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineAppointmentSystem.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.AppointmentViewModels
{
    public class AppointmentEditViewModel
    {
        public int AppointmentId { get; set; }

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
        public string Notes { get; set; }

        [Display(Name = "Durum")]
        public AppointmentStatus Status { get; set; }

        // Dropdown listeleri için
        public SelectList Services { get; set; }
        public SelectList Employees { get; set; }
        public SelectList Statuses { get; set; }
    }
}