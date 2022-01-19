using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;

namespace WebClient.Repositories.Interfaces
{
    public interface IDotTiemVaccineRepository : IBaseRepository<DotTiemVaccine>
    {
        /// <summary>
        /// Tìm kiếm đợt tiêm
        /// </summary>
        /// <param name="pagingRequest">Paging request</param>
        /// <returns>Danh sách đợt tiêm</returns>
        Task<PagingResponse<DotTiemVaccineInfo>> SearchAsync(PagingRequest<DotTiemVaccineFilterVM> pagingRequest, int idDonVi);

        /// <summary>
        /// Lay danh sach dot tiem Vaccine
        /// </summary>
        /// <param name="id_Cha">id DotTiem Cha</param>
        /// <returns>List dot tiem Vaccine</returns>
        Task<PagingResponse<DotTiemVaccineInfo>> LayDanhSachDotTiemVaccineTheoIdCha(PagingRequest<DotTiemVaccineFilterVM> pagingRequest);
        /// Lay danh sach doi tuong dang ky tiem vaccine theo id_dottiem
        /// </summary>
        /// <param name="id_DotTiem">id DotTiem_Vaccine</param>
        /// <returns>List ThongTin_NguoiDan</returns>
        Task<IEnumerable<ThongTin_NguoiDanVM>> GetAllDoiTuongDangKyTiemTheoIdDotTiem(int id_DotTiem);
        Task<IEnumerable<ThongTin_DangKy_Tiem_Vaccine>> GetThongTin_DangKy_Tiem_Vaccine(int id);

        /// <summary>
        /// Lay danh sach cac dot tiem cua phuong theo id dot tiem cua Quan
        /// </summary>
        /// <param name="id_DotTiem">id dot tiem cua Quan</param>
        /// <returns></returns>
        Task<IEnumerable<DotTiemVaccine>> LayDsDotTiemTheoIdCha(int id_DotTiem);

        /// <summary>
        /// Chốt danh sách đăng ký của đợt tiêm và đợt tiêm con
        /// </summary>
        /// <param name="idDotTiem">id đợt tiêm</param>
        /// <param name="account">thông tin người cập nhật</param>
        /// <returns>không trả về</returns>
        Task ChotDanhSachDotTiem(int idDotTiem, AccountInfo account);
    }
}
