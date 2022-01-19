using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;

namespace WebClient.Repositories.Interfaces
{
    public interface IDmDoiTuongUuTienRepository : IBaseRepository<DmDoiTuongUuTien>
    {
        /// <summary>
        /// Lấy tất cả danh sách đối tượng ưu tiên
        /// </summary>
        /// <returns>Danh sách đối tượng ưu tiên</returns>
        Task<IEnumerable<DmDoiTuongUuTien>> GetAllAsync();
    }
}
