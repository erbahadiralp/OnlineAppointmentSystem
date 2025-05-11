using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.AccountViewModels
{
    public class ProfileEditViewModel
    {
        [Required(ErrorMessage = "Ad gereklidir")] 
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gereklidir")] 
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Telefon gereklidir")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Adres")]
        public string Address { get; set; }
    }
} 