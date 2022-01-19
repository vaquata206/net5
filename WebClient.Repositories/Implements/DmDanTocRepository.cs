using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class DmDanTocRepository : BaseRepository<DmDanToc>, IDmDanTocRepository
    {
        public DmDanTocRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Lấy tất cả danh sách dân tộc
        /// </summary>
        /// <returns>Danh sách dân tộc</returns>
        public async Task<IEnumerable<DmDanToc>> GetAllAsync()
        {
            var sql = "SELECT * FROM DM_DANTOC WHERE Tinh_Trang = :Tinh_Trang";
            var parameters = new
            {
                Tinh_Trang = Constants.States.Actived.GetHashCode()
            };
            var list = await this.dbContext.QueryAsync<DmDanToc>(sql: sql, param: parameters);
            return list;
        }
    }
}
