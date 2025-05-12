using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir")]
        [StringLength(100, ErrorMessage = "Şifre en az {2} karakter uzunluğunda olmalıdır", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi Onayla")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; }
    }
} 