using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.EmployeeViewModels
{
    public class EmployeeEditViewModel
    {
        public int EmployeeId { get; set; }

        [Display(Name = "Ünvan")]
        public string Title { get; set; }

        [Display(Name = "Departman")]
        public string Department { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "En az bir hizmet seçmelisiniz.")]
        [Display(Name = "Hizmetler")]
        public List<int> ServiceIds { get; set; }

        // Dropdown listeleri için
        [ValidateNever]
        public MultiSelectList Services { get; set; }

        // Kullanıcı bilgileri
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon alanı zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        public string UserId { get; set; }
    }
}