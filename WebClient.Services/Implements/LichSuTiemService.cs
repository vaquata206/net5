using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Repositories;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class LichSuTiemService : ILichSuTiemService
    {
        private readonly IUnitOfWork unitOfWork;

        public LichSuTiemService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Lấy danh sách lịch sử tiêm vaccine theo id người đăng ký
        /// </summary>
        /// <param name="idNguoiDangKy">id người đăng ký tiêm</param>
        /// <returns>danh sách lịch sử tiêm</returns>
        public async Task<IEnumerable<LichSu_Tiem_Vaccine>> LayDsLichSuTiemTheoIdNguoiDangKy(int idNguoiDangKy)
        {
            return await this.unitOfWork.LichSuTiemVaccineRepository.LayDsLichSuTiemTheoIdNguoiDangKy(idNguoiDangKy);
        }
    }
}
