using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.WorkingHoursViewModels
{
    public class WorkingHoursViewModel
    {
        public int WorkingHoursId { get; set; }

        [Required(ErrorMessage = "Çalışan seçimi gereklidir")]
        [Display(Name = "Çalışan")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Gün seçimi gereklidir")]
        [Display(Name = "Gün")]
        [Range(0, 6, ErrorMessage = "Geçerli bir gün seçiniz")]
        public int DayOfWeek { get; set; }

        [Required(ErrorMessage = "Başlangıç saati gereklidir")]
        [Display(Name = "Başlangıç Saati")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Bitiş saati gereklidir")]
        [Display(Name = "Bitiş Saati")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        // Dropdown listeleri için
        public SelectList Employees { get; set; }
        public SelectList Days { get; set; }
    }
}