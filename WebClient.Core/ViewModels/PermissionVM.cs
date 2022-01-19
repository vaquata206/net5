using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class PermissionVM
    {
        public int? Id_Quyen { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ten_Quyen { get; set; }

        [MaxLength(200)]
        public string Ghi_Chu { get; set; }
    }
}
