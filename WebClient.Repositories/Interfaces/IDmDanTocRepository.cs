using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IDmDanTocRepository : IBaseRepository<DmDanToc>
    {
        /// <summary>
        /// Lấy tất cả danh sách dân tộc
        /// </summary>
        /// <returns>Danh sách dân tộc</returns>
        Task<IEnumerable<DmDanToc>> GetAllAsync();
    }
}
