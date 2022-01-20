using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IPermissionFeatureRepository : IBaseRepository<ChucNangQuyen>
    {
        /// <summary>
        /// delete permission feature by id permission and id feature
        /// </summary>
        /// <param name="permission_Feature">infor permission feature</param>
        /// <returns>no reuturn</returns>
        Task DeleteAsync(ChucNangQuyen permission_Feature);

        /// <summary>
        /// get list permission features by id of permission
        /// </summary>
        /// <param name="permissionId">id of permission</param>
        /// <returns>list of permission features</returns>
        Task<IEnumerable<ChucNangQuyen>> GetListsByPermissionIdAsync(int permissionId);
    }
}
