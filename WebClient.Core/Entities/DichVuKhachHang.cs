using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.Core.Entities
{
    public class DichVuKhachHang : BaseEntity
    {
        public int IdKhachHang { get; set; }
        public int IdDichVu { get; set; }
        public decimal GiaCuoc { get; set; }
        public DateTime NgayDangKy { get; set; }
        public int TrangThai { get; set; }
        public string GhiChu { get; set; }
    }
}
