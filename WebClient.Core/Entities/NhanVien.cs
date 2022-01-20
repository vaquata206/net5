using System;
using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("NhanVien")]
    public class NhanVien : BaseEntity
    {
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string IdVaiTro { get; set; }
    }
}
