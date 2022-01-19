using System;

namespace WebClient.Core.ViewModels
{
    public class DotTiemVaccineFilterVM
    {
        public int Id_DotTiem { get; set; }
        public string TenDotTiem { get; set; }
        public DateTime? NgayTiem { get; set; }
        public int TrangThai { get; set; }
        public int TinhTrang { get; set; }
    }
}
