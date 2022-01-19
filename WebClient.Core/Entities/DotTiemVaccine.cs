using System;
using Dapper.Contrib.Extensions;
namespace WebClient.Core.Entities
{
    [Table("DotTiem_Vaccine")]
    public class DotTiemVaccine : BaseEntity
    {
        [ExplicitKey]
        public int Id_DotTiem { get; set; }
        public string Ma_DotTiem { get; set; }
        public string Ten_KeHoach { get; set; }
        public DateTime NgayTiem_BatDau { get; set; }
        public DateTime NgayTiem_KetThuc { get; set; }
        public int Trang_Thai { get; set; }
        public int SoLuong_DangKy { get; set; }
        public int SoLuong_DaTiem { get; set; }
        public int Id_DonVi { get; set; }
        public int Id_Cha { get; set; }
        public int TinhTrang_DangKy { get; set; }
        public DateTime? HanCuoi_DangKy { get; set; }
        public DateTime? Ngay_DangKy { get; set; }
    }
}
