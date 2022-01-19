using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Services.Interfaces
{
    public interface IPermissionFeatureService
    {
        /// <summary>
        /// set new list features for permission
        /// </summary>
        /// <param name="featureIds">list id of features</param>
        /// <param name="permissionId">id of permission</param>
        /// <param name="idNhanVien">id of handler</param>
        /// <returns>no return</returns>
        Task SetFeaturesForPermissionAsync(IEnumerable<int> featureIds, int permissionId, int idNhanVien);

        /// <summary>
        /// get: permission and features by permission id
        /// </summary>
        /// <param name="permissionId">id of permission</param>
        /// <returns>list of permission features</returns>
        Task<IEnumerable<Permission_Feature>> GetPermissionFeaturesByPermissionId(int permissionId);
    }
}
