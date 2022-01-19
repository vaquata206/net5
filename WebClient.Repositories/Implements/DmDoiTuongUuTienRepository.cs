using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.Models;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class DmDoiTuongUuTienRepository : BaseRepository<DmDoiTuongUuTien>, IDmDoiTuongUuTienRepository
    {
        public DmDoiTuongUuTienRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Lấy tất cả danh sách đối tượng ưu tiên
        /// </summary>
        /// <returns>Danh sách đối tượng ưu tiên</returns>
        public async Task<IEnumerable<DmDoiTuongUuTien>> GetAllAsync()
        {
            var sql = "SELECT * FROM DM_DOITUONG_UUTIEN WHERE Tinh_Trang = :Tinh_Trang";
            var parameters = new
            {
                Tinh_Trang = Constants.States.Actived.GetHashCode()
            };
            var list = await this.dbContext.QueryAsync<DmDoiTuongUuTien>(sql: sql, param: parameters);
            return list;
        }
    }
}
