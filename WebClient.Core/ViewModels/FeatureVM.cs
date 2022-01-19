using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class FeatureVM
    {
        public int? ID_CN { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ten_CN { get; set; }

        [MaxLength(500)]
        public string Mota_CN { get; set; }

        [MaxLength(255)]
        public string ToolTip_CN { get; set; }
        public int? ID_CN_PR { get; set; }

        [MaxLength(50)]
        public string Controller_Name { get; set; }

        [MaxLength(50)]
        public string Action_Name { get; set; }

        public bool HienThi_Menu { get; set; }
    }
}
