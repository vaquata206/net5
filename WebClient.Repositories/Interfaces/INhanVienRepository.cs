using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface INhanVienRepository : IBaseRepository<NhanVien>
    {
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
        Task<IEnumerable<NhanVien>> GetNhanVienKyThuat();
    }
}
