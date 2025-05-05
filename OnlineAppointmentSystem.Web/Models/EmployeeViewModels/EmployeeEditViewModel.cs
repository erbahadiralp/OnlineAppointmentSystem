using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.EmployeeViewModels
{
    public class EmployeeEditViewModel
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Ünvan gereklidir")]
        [Display(Name = "Ünvan")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Departman gereklidir")]
        [Display(Name = "Departman")]
        public string Department { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        [Display(Name = "Hizmetler")]
        public List<int> ServiceIds { get; set; }

        // Dropdown listeleri için
        public MultiSelectList Services { get; set; }

        // Kullanıcı bilgileri
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }
    }
}