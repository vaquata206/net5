using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("ChucNang")]
    public class Feature
    {
        [Key]
        public int Id { get; set; }
        public string TenChucNang { get; set; }
        public string MoTa { get; set; }
        public int IdCha { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int ThuTu { get; set; }
        public int HienThi { get; set; }
        public string Url { get; set; }
        public bool DaXoa { get; set; }
        public int KichHoat { get; set; }
        public int NguoiKhoiTao { get; set; }
        public DateTime NgayKhoiTao { get; set; }
        public int? NguoiCapNhat { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string Icon { get; set; }

        [Computed]
        public IEnumerable<Feature> Children { get; set; }
    }
}
