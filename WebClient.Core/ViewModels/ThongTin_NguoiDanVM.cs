using WebClient.Core.Entities;

namespace WebClient.Core.ViewModels
{
    public class ThongTin_NguoiDanVM : ThongTin_NguoiDan
    {
        public string Ten_GioiTinh { get; set; }
        public string Nam_Sinh { get; set; }
        public int Trang_Thai { get; set; }
        public int TinhTrang_TiemChung { get; set; }
        public string Ghi_Chu_DangKyTiem { get; set; }
        public int Id_DangKy { get; set; }
        public string Ma_QuanHuyen { get; set; }
        public string Ten_QuanHuyen { get; set; }
        public string Ma_PhuongXa { get; set; }
        public string Ten_PhuongXa { get; set; }
        public LichSuTiemVaccineVM LichSuTiem { get; set; }
    }
}
