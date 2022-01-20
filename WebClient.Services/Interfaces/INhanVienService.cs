using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    public interface INhanVienService
    {
        /// <summary>
        /// get employee view model by id nhanvien
        /// </summary>
        /// <param name="employeeId">employee's id</param>
        /// <returns>the employee instance</returns>
        Task<NhanVien> GetByIdAsync(int employeeId);

        /// <summary>
        /// update profile
        /// </summary>
        /// <param name="nhanVienVM">the employee VM</param>
        /// <param name="userId">user id</param>
        /// <returns>A task</returns>
        Task UpdateProfileAsync(NhanVienVM nhanVienVM, int userId);

        /// <summary>
        /// Lay danh sach nhan vien theo id don vi
        /// </summary>
        /// <param name="idDonVi">id don vi</param>
        /// <returns>danh sach nhan vien</returns>
        Task<IEnumerable<NhanVien>> LayDsNhanVienTheoIdDonVi(int idDonVi);

        /// <summary>
        /// Get employees by chức vụ
        /// </summary>
        /// <param name="chucVu">Chức vụ</param>
        /// <returns>List employee</returns>
        Task<IEnumerable<NhanVien>> GetEmployeesByChucVuAsync(int chucVu);

        /// <summary>
        /// Xoa thong tin nhan vien
        /// </summary>
        /// <param name="idNhanVien">id nhan vien</param>
        /// <param name="userId">Current user id</param>
        /// <returns>the Task</returns>
        Task DeleteByIdAsync(int idNhanVien, int userId);

        /// <summary>
        /// Luu thong tin nhan vien (insert or update)
        /// </summary>
        /// <param name="nhanVienVM">nhan vien VM</param>
        /// <param name="userId">the current userid</param>
        /// <returns>the task</returns>
        Task SaveAsync(NhanVienVM nhanVienVM, int userId);
    }
}
