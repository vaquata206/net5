using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(60)]
        public string Ma { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ten { get; set; }

        [Required]
        [MaxLength(200)]
        public string DiaChi { get; set; }
        [Required]
        [MaxLength(40)]
        public string SoDienThoai { get; set; }


        public int DonViCha { get; set; }

        [MaxLength(200)]
        public string GhiChu { get; set; }
    }
}
