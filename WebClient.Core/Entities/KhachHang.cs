using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("KhachHang")]
    public class KhachHang : BaseEntity
    {
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
    }
}
