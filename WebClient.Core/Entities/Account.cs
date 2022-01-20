using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("TaiKhoan")]
    public class Account : BaseEntity
    {
        public string TenTaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public int? IdNhanVien { get; set; }
        public int? IdKhachHang { get; set; }
        public bool IsKhachHang { get; set; }
    }
}
