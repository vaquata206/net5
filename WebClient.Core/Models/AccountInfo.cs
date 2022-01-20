namespace WebClient.Core.Models
{
    public class AccountInfo
    {
        public int Id { get; set; }
        public string TenTaiKhoan { get; set; }
        public string HoTen { get; set; }
        public bool IsKhachHang { get; set; }
        public int? IdNhanVien { get; set; }
        public int? IdKhachHang { get; set; }
        public int? IdVaiTro { get; set; }
    }
}
