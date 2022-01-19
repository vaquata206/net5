using Dapper.Contrib.Extensions;
using System;

namespace WebClient.Core.Entities
{
    [Table("ThongTin_NguoiDan")]
    public class ThongTin_NguoiDan : BaseEntity
    {
        [ExplicitKey]
        public int Id_ThongTin { get; set; }
        public string Ho_Ten { get; set; }
        public int Gioi_Tinh { get; set; }
        public DateTime Ngay_Sinh { get; set; }
        public string Email { get; set; }
        public string Nghe_Nghiep { get; set; }
        public string DonVi_CongTac { get; set; }
        public string So_DienThoai { get; set; }
        public string CMND { get; set; }
        public string SoThe_BHYT { get; set; }
        public int Id_DoiTuong_UuTien { get; set; }
        public int Id_DanToc { get; set; }
        public string Quoc_Tich { get; set; }
        public int Id_DonVi { get; set; }
        public string DiaChi_HienTai { get; set; }
        public int SoMuiDaTiem { get; set; }
    }
}
