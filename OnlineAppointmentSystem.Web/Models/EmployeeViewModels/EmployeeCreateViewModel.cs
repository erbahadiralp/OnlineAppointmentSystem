using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnlineAppointmentSystem.Web.Models.EmployeeViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon alanı zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Adres alanı zorunludur.")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrar alanı zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Unvan")]
        public string Title { get; set; }

        [Display(Name = "Departman")]
        public string Department { get; set; }

        [Display(Name = "Hizmet")]
        public int? ServiceId { get; set; }

        [Required(ErrorMessage = "Rol seçmelisiniz.")]
        [Display(Name = "Rol")]
        public string Role { get; set; }

        [ValidateNever]
        public SelectList Services { get; set; }

        public string UserId { get; set; }
    }
}