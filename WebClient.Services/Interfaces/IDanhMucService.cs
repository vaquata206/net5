using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;

namespace WebClient.Services.Interfaces
{
    public interface IDanhMucService
    {
        /// <summary>
        /// Lấy tất cả danh sách đối tượng ưu tiên
        /// </summary>
        /// <returns>danh sách đối tượng ưu tiên</returns>
        Task<IEnumerable<DmDoiTuongUuTienInfo>> GetAllDmDoiTuongUuTienAsync();

        /// <summary>
        /// Lấy tất cả danh sách dân tộc
        /// </summary>
        /// <returns>danh sách dân tộc</returns>
        Task<IEnumerable<DmDanTocInfo>> GetAllDmDanTocAsync();
    }
}
