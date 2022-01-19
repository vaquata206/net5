using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface ILichSuTiemVaccineRepository : IBaseRepository<LichSu_Tiem_Vaccine>
    {
        /// <summary>
        /// Luu danh sach ket qua tiem vao Db
        /// </summary>
        /// <param name="listKetQuaTiem">danh sach thong tin ket qua tiem</param>
        /// <returns></returns>
        Task LuuDanhSachKetQuaTiem(List<LichSu_Tiem_Vaccine> listKetQuaTiem);

        /// <summary>
        /// Lấy danh sách lịch sử tiêm vaccine theo id người đăng ký
        /// </summary>
        /// <param name="idNguoiDangKy">id người đăng ký tiêm</param>
        /// <returns>danh sách lịch sử tiêm</returns>
        Task<IEnumerable<LichSu_Tiem_Vaccine>> LayDsLichSuTiemTheoIdNguoiDangKy(int idNguoiDangKy);
    }
}
