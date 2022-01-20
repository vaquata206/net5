using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    public interface IBaoHongService
    {
        Task<IEnumerable<TrangThaiPhieu>> GetTrangThai();
        Task<IEnumerable<BaoHongInfo>> SearchAsync(BaoHongSearch search);
        Task<IEnumerable<DichVu>> GetDichVuByKhachHangId(int idKhachHang);
        Task SaveAsync(BaoHongVM viewModal, int idKhachHang);
        Task<PhieuBaoHong> GetByIdAsync(int id);
        Task<DichVuKhachHang> GetDichVuKhachHangByIdAsync(int idDichVuKhachHang);
        Task<IEnumerable<NhanVien>> GetNhanVienKyThuat();
        Task TiepNhan(int id, int idNhanVien);
        Task ChuyenKyThuat(ChuyenKyThuatVM viewModal);
        Task<IEnumerable<ChiTietPhieuBaoHong>> GetChiTietBaoHong(int id);
        Task HoanThanh(int id, int idNhanVien);
        Task GuiDanhGia(DanhGiaVM danhGiaVM);
    }
}
