using System;

namespace WebClient.Core.Models
{
    public class DoiTuongDangKyTiemInfo
    {
        public int Id_ThongTin { get; set; }
        public string Ho_Ten { get; set; }
        public int Gioi_Tinh { get; set; }
        public DateTime Ngay_Sinh { get; set; }
        public string Cmnd { get; set; }
        public string DiaChi_HienTai { get; set; }
        public int SoMuiDaTiem { get; set; }
        public string So_DienThoai { get; set; }
        public int Id_DoiTuong_UuTien { get; set; }
        public string Ma_DoiTuong_UuTien { get; set; }
        public string NgheNghiepUuTien { get; set; }
        public string PhuongXa { get; set; }
    }
}
