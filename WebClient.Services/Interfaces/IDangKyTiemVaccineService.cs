using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    public interface IDangKyTiemVaccineService
    {
        /// <summary>
        /// Tìm kiếm đối tượng đăng ký tiêm vaccine
        /// </summary>
        /// <param name="pagingRequest">Paging request</param>
        /// <returns>Danh sách đối tượng đăng ký tiêm</returns>
        Task<PagingResponse<DoiTuongDangKyTiemInfo>> SearchAsync(PagingRequest<DoiTuongDangKyTiemFilterVM> pagingRequest);

        /// <summary>
        /// Lấy thông tin người đăng ký tiêm vaccine theo id người đăng ký
        /// </summary>
        /// <param name="id">id người đăng ký</param>
        /// <returns>Thông tin chi tiết người đăng ký</returns>
        Task<ThongTin_NguoiDan> GetByIdASync(int id);

        /// <summary>
        /// Cập nhật thông tin người đăng ký tiêm vaccine
        /// </summary>
        /// <param name="thongTinNguoiDanVM">thông tin cập nhật</param>
        /// <param name="account">thông tin người cập nhật</param>
        /// <returns>không trả về</returns>
        Task SaveAsync(ThongTinNguoiDanVM thongTinNguoiDanVM, AccountInfo account);

        /// <summary>
        /// Xóa: Cập nhật tình trạng của thông tin người đăng ký tiêm vaccine theo id
        /// </summary>
        /// <param name="id">id người đăng ký</param>
        /// <param name="account">thông tin người cập nhật</param>
        /// <returns>không trả về</returns>
        Task DeleteAsync(int id, AccountInfo account);

        /// <summary>
        /// Thong ke tong quat so luong lieu vaccine, so nguoi dang ky, so nguoi da tiem  mui 1, 2
        /// </summary>
        /// <param name="id_DonVi">id don vi thong ke</param>
        /// <returns></returns>
        Task<ThongKeTongQuatVM> ThongKeTongQuatSoLuong(int id_DonVi);

        /// <summary>
        /// Import danh sách đăng ký tiêm vaccine
        /// </summary>
        /// <param name="FileDSDKTiem">File excel danh sách đăng ký tiêm vaccine</param>
        /// <param name="account">User</param>
        /// <returns></returns>
        Task<IEnumerable<ThongTin_NguoiDan>> DongBoDSDKTiemFromExcel(IFormFile FileDSDKTiem, AccountInfo account, bool commit = true);
    }
}
