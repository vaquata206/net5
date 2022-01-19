using System;

namespace WebClient.Core.ViewModels
{
    public class DoiTuongDangKyTiemFilterVM
    {
        public string HoTen { get; set; }
        public string NgaySinh { get; set; }
        public string SoDienThoai { get; set; }
        public string Cmnd { get; set; }
        public int[] DsDoiTuongUuTien { get; set; }
        public int SoMuiDaTiem { get; set; }
        public int GioiTinh { get; set; }
        public int IdDonVi { get; set; }
    }
}
