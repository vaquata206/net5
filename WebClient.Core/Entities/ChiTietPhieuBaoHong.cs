using System;
using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("ChiTietPhieuBaoHong")]
    public class ChiTietPhieuBaoHong
    {
        [Key]
        public int Id { get; set; }
        public int IdPhieuBaoHong { get; set; }
        public int IdTrangThaiPhieu { get; set; }
        public DateTime ThoiGian { get; set; }
        public int IdNhanVien { get; set; }
    }
}
