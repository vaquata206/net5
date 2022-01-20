using System.Data;
using System.Threading.Tasks;

namespace WebClient.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        Task<TEntity> GetByIdAsync(int id, bool checkTinhTrang = true);
        Task<T> AddAsync<T>(T entity) where T : class;
        Task UpdateAsync<T>(T entity) where T : class;
        Task DeleteAsync<T>(T entity) where T : class;
        Task<string> TaoMa(string baseCode);
    }
}
