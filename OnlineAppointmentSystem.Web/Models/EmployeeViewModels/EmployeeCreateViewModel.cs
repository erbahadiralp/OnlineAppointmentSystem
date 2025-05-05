using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.EmployeeViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required(ErrorMessage = "Kullanıcı seçimi gereklidir")]
        [Display(Name = "Kullanıcı")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Ünvan gereklidir")]
        [Display(Name = "Ünvan")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Departman gereklidir")]
        [Display(Name = "Departman")]
        public string Department { get; set; }

        [Display(Name = "Hizmetler")]
        public List<int> ServiceIds { get; set; }

        // Dropdown listeleri için
        public SelectList Users { get; set; }
        public MultiSelectList Services { get; set; }
    }
}