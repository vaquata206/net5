using System;

namespace WebClient.Core.ViewModels
{
    public class DotTiemVaccineVM
    {
        public int Id_DotTiem { get; set; }
        public string Ma_DotTiem { get; set; }
        public string Ten_KeHoach { get; set; }
        public string Ten_DonVi;
        public DateTime NgayTiem_BatDau { get; set; }
        public DateTime NgayTiem_KetThuc { get; set; }
        public int Trang_Thai { get; set; }
        public int SoLuong_DangKy { get; set; }
        public int SoLuong_DaTiem { get; set; }

        public int Id_DonVi { get; set; }
        public int Id_Cha { get; set; }
        public int TinhTrang_DangKy { get; set; }
        public string Ghi_Chu { get; set; }

        public int[] ds_Id_DotTiem { get; set; }
        public int[] ds_Id_DonVi { get; set; }
        public int[] ds_SoLuong_DangKy { get; set; }
    }
}
