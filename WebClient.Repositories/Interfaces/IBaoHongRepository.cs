using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;

namespace WebClient.Repositories.Interfaces
{
    public interface IBaoHongRepository : IBaseRepository<PhieuBaoHong>
    {
        Task<IEnumerable<TrangThaiPhieu>> GetTrangThai();
        Task<IEnumerable<BaoHongInfo>> SearchAsync(BaoHongSearch search);
        Task<IEnumerable<DichVu>> GetDichVuByKhachHangId(int idKhachHang);
        Task<DichVuKhachHang> GetDichVuKhachHang(int idKhachHang, int idDichVu);
        Task<DichVuKhachHang> GetDichVuKhachHangByIdAsync(int idDichVuKhachHang);
        Task<IEnumerable<ChiTietPhieuBaoHong>> GetChiTietBaoHong(int id);
    }
}
