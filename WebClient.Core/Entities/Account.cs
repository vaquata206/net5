using System;
using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("Nguoi_dung")]
    public class Account : BaseEntity
    {
        [ExplicitKey]
        public int Id_NguoiDung { get; set; }
        public string Ma_NguoiDung { get; set; }
        public string UserName { get; set; }
        public string MatKhau { get; set; }
        public int Id_VaiTro { get; set; }
        public int Quan_Tri { get; set; }
        public int Id_NhanVien { get; set; }
        public int SoLan_LoginSai { get; set; }
        public DateTime? Ngay_Login { get; set; }
        public DateTime? Ngay_DoiMatKhau { get; set; }
        public int Trang_Thai { get; set; }

        [Computed]
        public Department Department { get; set; }
    }
}
