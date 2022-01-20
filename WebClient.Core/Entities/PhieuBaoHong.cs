using System;
using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("PhieuBaoHong")]
    public class PhieuBaoHong : BaseEntity
    {
        public int IdDichVuKhachHang { get; set; }
        public int IdTrangThaiPhieu { get; set; }
        public string MoTa { get; set; }
        public int TinhTrang { get; set; }
        public DateTime NgayKhoiTao { get; set; }
        public int? DiemDanhGia { get; set; }
        public string NoiDungDanhGia { get; set; }
        public DateTime? NgayDanhGia { get; set; }
    }
}
