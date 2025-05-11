using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.WorkingHoursViewModels
{
    public class WorkingHoursViewModel
    {
        public int WorkingHoursId { get; set; }

        [Required(ErrorMessage = "Lütfen bir çalışan seçiniz")]
        [Display(Name = "Çalışan")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Lütfen en az bir gün seçiniz")]
        [Display(Name = "Günler")]
        public List<int> SelectedDays { get; set; } = new List<int>();

        [Required(ErrorMessage = "Lütfen başlangıç saati giriniz")]
        [Display(Name = "Başlangıç Saati")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Lütfen bitiş saati giriniz")]
        [Display(Name = "Bitiş Saati")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        // Dropdown listeler için - validation'dan çıkarıldı
        [ValidateNever]
        public SelectList Employees { get; set; }

        [ValidateNever]
        public SelectList Days { get; set; }
    }
}