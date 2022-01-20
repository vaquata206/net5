using Dapper.Contrib.Extensions;
using System;

namespace WebClient.Core.Entities
{
    [Table("ChucNangQuyen")]
    public class ChucNangQuyen
    {
        public int Id { get; set; }
        public int IdQuyen { get; set; }
        public int IdChucNang { get; set; }
        public bool DaXoa { get; set; }
        public int NguoiKhoiTao { get; set; }
        public int? NguoiCapNhat { get; set; }
        public DateTime NgayKhoiTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
    }
}
