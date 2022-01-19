using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IPermissionFeatureRepository : IBaseRepository<Permission_Feature>
    {
        /// <summary>
        /// delete permission feature by id permission and id feature
        /// </summary>
        /// <param name="permission_Feature">infor permission feature</param>
        /// <returns>no reuturn</returns>
        Task DeleteAsync(Permission_Feature permission_Feature);

        /// <summary>
        /// get list permission features by id of permission
        /// </summary>
        /// <param name="permissionId">id of permission</param>
        /// <returns>list of permission features</returns>
        Task<IEnumerable<Permission_Feature>> GetListsByPermissionIdAsync(int permissionId);
    }
}
