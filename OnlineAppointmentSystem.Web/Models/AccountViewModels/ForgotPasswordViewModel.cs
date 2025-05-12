using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "E-posta adresi gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }
    }
} 