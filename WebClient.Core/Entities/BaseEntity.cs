using System;

namespace WebClient.Core.Entities
{
    public class BaseEntity
    {
        public int Tinh_Trang { get; set; }
        public int Id_NV_KhoiTao { get; set; }
        public int? Id_NV_CapNhat { get; set; }
        public DateTime Ngay_KhoiTao { get; set; }
        public DateTime? Ngay_CapNhat { get; set; }
        public string Ghi_Chu { get; set; }
    }
}
