using System.ComponentModel.DataAnnotations;

namespace OnlineAppointmentSystem.Web.Models.ServiceViewModels
{
    public class ServiceCreateViewModel
    {
        [Required(ErrorMessage = "Hizmet adı gereklidir")]
        [Display(Name = "Hizmet Adı")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Açıklama gereklidir")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fiyat gereklidir")]
        [Display(Name = "Fiyat")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Süre gereklidir")]
        [Display(Name = "Süre (dakika)")]
        [Range(1, int.MaxValue, ErrorMessage = "Süre 1 dakikadan az olamaz")]
        public int Duration { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
    }
}