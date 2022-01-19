using System;
using Dapper.Contrib.Extensions;

namespace WebClient.Core.Entities
{
    [Table("Nhan_Vien")]
    public class Employee : BaseEntity
    {
        [ExplicitKey]
        public int Id_NhanVien { get; set; }
        public string Ma_NhanVien { get; set; }
        public string Ho_Ten { get; set; }
        public string Dia_Chi { get; set; }
        public string Dien_Thoai { get; set; }
        public string Email { get; set; }
        public DateTime? Nam_Sinh { get; set; }
        public string So_CMND { get; set; }
        public DateTime? NgayCap_CMND { get; set; }
        public string NoiCap_CMND { get; set; }
        public int Id_DonVi { get; set; }
        public int Chuc_Vu { get; set; }
        public int Trang_Thai { get; set; }
    }
}
