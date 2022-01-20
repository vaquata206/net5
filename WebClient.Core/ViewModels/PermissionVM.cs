using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class PermissionVM
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TenQuyen { get; set; }

        [MaxLength(200)]
        public string GhiChu { get; set; }
    }
}
