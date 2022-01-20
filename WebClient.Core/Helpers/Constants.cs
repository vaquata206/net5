namespace WebClient.Core.Helpers
{
    public static class Constants
    {
        public enum States
        {
            Disabed = 0,
            Actived = 1,
            Blocked = 2
        }

        public static class TrangThai
        {
            public static bool DaXoa = true;
            public static bool ChuaXoa = false;
        }

        public static string StrongPasswordRegex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[&;!*#@$])[0-9a-zA-Z].{5,}$";
        public static class ResetPassword
        {
            public static int LengthPassword = 8;
            public static string TitleEmail = "Khôi phục lại mật khẩu cho tài khoản tại Bản đồ số Khu Công nghệ cao Đà Nẵng";

        }

        public static class EmailHeThong
        {
            public const string Username = "bandosokcncdn@gmail.com";
            public const string Password = "Vnpt@2020";
            public const string HostMail = "smtp.gmail.com";
            public const int PortMail = 587;
        }

        public enum NguoiKhoiTao
        {
            HeThong = 0
        }

        public enum LoaiTaiKhoan
        {
            NhaDauTu = 1,
            NhanVien = 2
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

        public static string[] DinhDangTaiLieu = new string[10] { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".csv", ".png", ".jpg" };
        public static string[] DinhDangHinhAnh = new string[3] { ".jpeg", ".png", ".jpg" };
        public static string DinhDangFileKML = ".kml";

        public static class DuongDanThuMuc
        {
            public static string HinhAnh = "/uploads/hinhanh/";
            public static string KML = "/uploads/kml/";
            public static string VanBan = "/uploads/vanban/";
            public static string BaoCaoNhaDauTu = "/assets/uploads/baocaonhadautu/";
            public static string YeuCauBaoCao = "/assets/uploads/yeucaubaocao/";
        }

        public static class LoaiTaiLieu
        {
            public static int HinhAnh = 1;
        }

        public static class SapXepVanBan
        {
            public static int NgayBanHanh = 0;
            public static int SoVanBan = 1;
        }

        public static string LienHeLink = "https://dhpiza.danang.gov.vn/web/guest/thong-tin-lien-he";

        public static class InformationFCM
        {
            public static string ServerKey = "AAAAh8MJjBk:APA91bH7X7wqE3nn4p8k5eMlPKM-Wm3hShWw6BP5A1eRk4Lsx80BUkF_y0_LiN1TThmElvQKN1uG-sjV5raizhDBKHCzp1KFZOU9gcou-Ij5RvigSZybPWbk-1ULIm60FEy2B4JmPlKa";
            public static string SenderId = "583092767769";
        }

        public static class TrangThaiBaoCao
        {
            public static int ChuaBaoCao = 0;
            public static int DaBaoCao = 1;
        }

        public static class PhanQuyen 
        {
            public static int NhaDauTu = 1002;
        }

        public static string LinkBanDoDefault = "/uploads/kml/default.kml.json";
    }
}
