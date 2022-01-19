namespace WebClient.Core.Helpers
{
    public static class Constants
    {
        public enum States
        {
            Disabed = 0,
            Actived = 1
        }

        public enum ChucVu
        {
            NhanVien = 0,
            ChuyenVien = 1,
            LanhDao = 2
        }

        public static readonly string FormatDate = "dd/MM/yyyy";
        public static readonly string FormatDateTime = "HH:mm:ss dd/MM/yyyy";
        
        public enum GioiTinh
        {
            Nam = 1,
            Nu = 2,
        }

        public static class SttHangCotExcelKetQuaTiem
        {
            public static int HoTen = 2;
            public static int NgaySinh = 3;
            public static int GioiTinh = 4;
            public static int SoDienThoai = 5;
            public static int SoTheBHYT = 6;
            public static int DiaChi = 7;
            public static int CMND = 8;
            public static int NoiCongTac = 9;
            public static int SoMuiDaTiem = 10;
            public static int NgayTiem = 11;
            public static int NgayNhapThongTin = 12;
        }

        public static string[] DinhDangTaiLieu = new string[10] { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".csv", ".png", ".jpg"};
        
        public enum TrangThaiTiem
        {
            ChuaTiem = 0,
            MotMui = 1,
            HaiMui = 2
        }
        public static class DmTinhThanh
        {
            public static string Ma_Tinh = "48";
            public static string Ten_Tinh = "Đà Nẵng";
        }

        public enum TrangThai_DotTiem
        {
            MoDangKy = 1,
            DongDangKy = 0
        }

        public static class STTHangCotExcelDSDKTiem
        {
            public static int STT = 0;
            public static int Ho_Ten = 1;
            public static int Gioi_Tinh = 2;
            public static int Ngay_Sinh = 3;
            public static int Email = 4;
            public static int Nghe_Nghiep = 6;
            public static int DonVi_CongTac = 7;
            public static int So_DienThoai = 8;
            public static int CMND = 9;
            public static int SoThe_BHYT = 10;
            public static int Ma_DoiTuong_UuTien = 5;
            public static int Ma_DanToc = 11;
            public static int Quoc_Tich = 12;
            public static int Ma_DonVi = 18;
            public static int DiaChi_HienTai = 19;
            public static int SoMuiDKTiem = 21;
        }
    }
}
