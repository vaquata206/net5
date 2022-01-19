using System.Data;
using System.Threading.Tasks;

namespace WebClient.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        Task<TEntity> GetByIdAsync(int id, bool checkTinhTrang = true);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<T> AddObjectAsync<T>(T entity);
        Task UpdateObjectAsync<T>(T entity);
    }
}
