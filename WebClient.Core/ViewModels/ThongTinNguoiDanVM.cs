using System;
using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class ThongTinNguoiDanVM
    {
        [Required]
        public int Id_ThongTin { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ho_Ten { get; set; }

        [Required]
        public int Gioi_Tinh { get; set; }

        [Required]
        public string Ngay_Sinh { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Nghe_Nghiep { get; set; }

        [MaxLength(50)]
        public string DonVi_CongTac { get; set; }

        [Required]
        [MaxLength(50)]
        public string So_DienThoai { get; set; }

        [Required]
        [MaxLength(50)]
        public string CMND { get; set; }

        [MaxLength(50)]
        public string SoThe_BHYT { get; set; }

        [Required]
        public int Id_DoiTuong_UuTien { get; set; }

        [Required]
        public int Id_DanToc { get; set; }

        [Required]
        public string Quoc_Tich { get; set; }

        [Required]
        public int Id_DonVi { get; set; }

        [Required]
        [MaxLength(200)]
        public string DiaChi_HienTai { get; set; }

        [Required]
        public int SoMuiDaTiem { get; set; }
    }
}
