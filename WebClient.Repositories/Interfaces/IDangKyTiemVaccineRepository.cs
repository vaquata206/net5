using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;

namespace WebClient.Repositories.Interfaces
{
    public interface IDangKyTiemVaccineRepository : IBaseRepository<ThongTin_NguoiDan>
    {
        /// <summary>
        /// Tìm kiếm đối tượng đăng ký tiêm vaccine
        /// </summary>
        /// <param name="pagingRequest">Paging request</param>
        /// <returns>Danh sách đối tượng đăng ký tiêm</returns>
        Task<PagingResponse<DoiTuongDangKyTiemInfo>> SearchAsync(PagingRequest<DoiTuongDangKyTiemFilterVM> pagingRequest);

        /// <summary>
        /// Lay danh sach doi tuong dang ky tiem vaccine theo id_dottiem
        /// </summary>
        /// <param name="id_DotTiem">id DotTiem_Vaccine</param>
        /// <returns>List ThongTin_NguoiDan</returns>
        Task<IEnumerable<ThongTin_NguoiDanVM>> LayDsDoiTuongDangKyTiemTheoIdDotTiem(int id_DotTiem);

        /// <summary>
        /// Lay danh sach CMND cua doi tuong dang ky tiem vaccine theo id_dottiem
        /// </summary>
        /// <param name="id_DotTiem">id DotTiem_Vaccine</param>
        /// <returns>List ThongTinDangKyTiemVM</returns>
        Task<IEnumerable<ThongTinDangKyTiemVM>> LayDsCMNDCuaDoiTuongDangKyTheoIdDotTiem(int id_DotTiem);

        /// <summary>
        /// Lay lich su tiem vaccine
        /// </summary>
        /// <param name="muitiem">mui tiem</param>
        /// <param name="idThongTin">Id thong tin nguoi dang ky tiem</param>
        /// <returns>LichSuTiemVaccineVM</returns>
        Task<LichSuTiemVaccineVM> LayLichSuTiemVaccine(int muitiem, int idThongTin);
        Task<int> GetTongSoMuiDaTiemVaccine(int id);

        /// <summary>
        /// Thong ke tong quat so luong lieu vaccine, so nguoi dang ky, so nguoi da tiem  mui 1, 2
        /// </summary>
        /// <param name="id_DonVi">id don vi thong ke</param>
        /// <returns></returns>
        Task<ThongKeTongQuatVM> ThongKeTongQuatSoLuong(int id_DonVi);

        /// <summary>
        /// Lấy danh sách người dân
        /// </summary>
        /// <returns>Danh sách người dân</returns>
        Task<IEnumerable<ThongTin_NguoiDan>> GetAllAsync();

        /// <summary>
        /// Lưu danh sách đăng ký tiêm vaccine
        /// </summary>
        /// <param name="listThongTin_NguoiDan">Danh sách đăng ký tiêm vaccine</param>
        /// <returns></returns>
        Task DongBoDSDKTiemFromExcel(List<ThongTin_NguoiDan> listThongTin_NguoiDan);
    }
}
