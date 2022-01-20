using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class FeatureVM
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenChucNang { get; set; }
        [MaxLength(500)]
        public string Mota { get; set; }
        public int? IdCha { get; set; }
        [MaxLength(50)]
        public string ControllerName { get; set; }
        [MaxLength(50)]
        public string ActionName { get; set; }
        public string Url { get; set; }
        public bool HienThi { get; set; }
        public bool KichHoat { get; set; }
    }
}
