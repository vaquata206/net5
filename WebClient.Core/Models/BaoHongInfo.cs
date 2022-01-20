using System;

namespace WebClient.Core.Models
{
    public class BaoHongInfo
    {
        public int Id { get; set; }
        public string DichVu { get; set; }
        public int IdDichVu { get; set; }
        public DateTime NgayKhoiTao { get; set; }
        public int IdTrangThaiPhieu { get; set; }
        public string TrangThai { get; set; }
        public int? DiemDanhGia { get; set; }
        public int IdKhachHang { get; set; }
    }
}
