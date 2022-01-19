using Dapper.Contrib.Extensions;
using System;

namespace WebClient.Core.Entities
{
    [Table("LichSu_Tiem_Vaccine")]
    public class LichSu_Tiem_Vaccine : BaseEntity
    {
        [ExplicitKey]
        public int Id_LichSu { get; set; }
        public int Id_DangKyTiem { get; set; }
        public DateTime Ngay_Tiem { get; set; }
        public string Ten_KeHoach { get; set; }
        public string Ten_DoiTiem { get; set; }
        public string DiaDiem_Tiem { get; set; }
        public string Lo_Vaccine { get; set; }
        public string Loai_Vaccine { get; set; }
        public string PhanUng_SauTiem { get; set; }
        public int SoMui_DaTiem { get; set; }
        public DateTime NgayNhap_ThongTin { get; set; }
    }
}
