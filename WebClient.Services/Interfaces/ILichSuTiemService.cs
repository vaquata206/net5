using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Services.Interfaces
{
    public interface ILichSuTiemService
    {
        /// <summary>
        /// Lấy danh sách lịch sử tiêm vaccine theo id người đăng ký
        /// </summary>
        /// <param name="idNguoiDangKy">id người đăng ký tiêm</param>
        /// <returns>danh sách lịch sử tiêm</returns>
        Task<IEnumerable<LichSu_Tiem_Vaccine>> LayDsLichSuTiemTheoIdNguoiDangKy(int idNguoiDangKy);
    }
}
