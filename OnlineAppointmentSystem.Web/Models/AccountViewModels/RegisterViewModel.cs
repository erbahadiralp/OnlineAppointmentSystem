using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad gereklidir")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gereklidir")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email adresi gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası gereklidir")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}