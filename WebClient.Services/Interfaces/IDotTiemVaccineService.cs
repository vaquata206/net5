using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    public interface IDotTiemVaccineService
    {
        /// <summary>
        /// Tìm kiếm đợt tiêm
        /// </summary>
        /// <param name="pagingRequest">Paging request</param>
        /// <param name="idDonVi">id donvi</param>
        /// <returns>Danh sách đợt tiêm</returns>
        Task<PagingResponse<DotTiemVaccineInfo>> SearchAsync(PagingRequest<DotTiemVaccineFilterVM> pagingRequest, int idDonVi);

        /// <summary>
        /// Lay danh sach doi tuong dang ky tiem cua dot tiem vaccine theo id_dottiem
        /// </summary>
        /// <param name="id_DotTiem">id DotTiem_Vaccine</param>
        /// <returns>DotTiem_VaccineVM</returns>
        Task<DotTiem_VaccineVM> LayThongTinDangKyTiemTheoIdDotTiem(int id_DotTiem);

        /// <summary>
        /// Lay thong tin dot tiem vaccine theo id_dottiem
        /// </summary>
        /// <param name="id_DotTiem">id DotTiem_Vaccine</param>
        /// <returns>DotTiemVaccineVM</returns>
        Task<DotTiemVaccineVM> LayThongTinDotTiemTheoId(int id_DotTiem);

        /// <summary>
        /// Lay danh sach dot tiem Vaccine
        /// <param name="id_Cha">id DotTiem_Vaccine</param>
        /// </summary>
        /// <returns>List dot tiem Vaccine</returns>
        Task<PagingResponse<DotTiemVaccineInfo>> LayDanhSachDotTiemVaccineTheoIdCha(int id_Cha, int id_DonVi, int userId);

        /// <summary>
        /// Luu dot tiem Vaccine
        /// <param name="viewModel">viewModel</param>
        /// </summary>
        /// <returns>result</returns>
        Task SaveAsync(DotTiemVaccineVM viewModel, int userId);

        /// <summary>
        /// Edit ds dot tiem Vaccine cua Phuong
        /// <param name="dsDotTiem">viewModel</param>
        /// <param name="userId">id current user</param>
        /// </summary>
        /// <returns>result</returns>
        Task EditDsDotTiemPhuongAsync(List<DotTiemVaccineVM> dsDotTiem, int userId);

        /// <summary>
        /// Delete dot tiem Vaccine
        /// <param name="id">id Dottiem</param>
        /// <param name="userId">id current user</param>
        /// </summary>
        /// <returns>result</returns>
        Task DeleteByIdAsync(int id, int userId);

        /// Nhap du lieu ket qua sau khi tiem chung
        /// </summary>
        /// <param name="id_DotTiem">id DotTiemVaccine</param>
        /// <param name="file">file ket qua tiem</param>
        /// <param name="account">user</param>
        /// <returns></returns>
        Task NhapKetQuaTiem(int id_DotTiem, IFormFile file, AccountInfo account);

        /// <summary>
        /// Xuất file danh sách đăng ký tiêm
        /// </summary>
        /// <param name="id">id dottiem</param>
        /// <returns>file excel</returns>
        Task<MemoryStream> ExportDsDangKyTiem(int id);

        /// <summary>
        /// Get dot tiem vaccine theo id
        /// </summary>
        /// <param name="id">id dot tiem vaccine</param>
        /// <returns>dot tiem vaccine</returns>
        Task<DotTiemVaccine> GetByIdAsync(int id);
        Task LuuDanhSachTiemVaccine(int id, IEnumerable<int> listDangKy, int dangKy, int userId);
        Task HuyDangKyDotTiemVaccine(int id, AccountInfo account);

        /// <summary>
        /// Chốt danh sách đăng ký của đợt tiêm
        /// </summary>
        /// <param name="id">id đợt tiêm</param>
        /// <param name="account">thông tin người cập nhật</param>
        /// <returns>không trả về</returns>
        Task ChotDanhSachDotTiem(int id, AccountInfo account);
        Task DangKyExcel(int idDotTiem, IFormFile fileDSDKTiem, AccountInfo account);
    }
}
