using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class AccountVM
    {
        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string MatKhau { get; set; }

        [Required]
        public int IdNhanVien { get; set; }
    }
}
