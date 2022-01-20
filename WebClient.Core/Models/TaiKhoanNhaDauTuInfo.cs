using WebClient.Core.Entities;

namespace WebClient.Core.Models
{
    public class TaiKhoanNhaDauTuInfo : Account
    {
        public string TenNhaDauTu { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Website { get; set; }
        public string NguoiDaiDien { get; set; }
    }
}
