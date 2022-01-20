using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IFeatureRepository : IBaseRepository<Feature>
    {
        /// <summary>
        /// Gets all featuress
        /// </summary>
        /// <returns>A list feature</returns>
        Task<IEnumerable<Feature>> GetAllAsync();

        Task CapNhatThuTuChucNang(int idCha, int stt);
        /// <summary>
        /// Id of the feature that will be deleted
        /// </summary>
        /// <param name="featureId">Id of feature</param>
        /// <returns>A Task</returns>
        Task DeleteFeatureAsync(int featureId);

        /// <summary>
        /// Get list feature of permission by id of account
        /// </summary>
        /// <param name="accountId">id of account</param>
        /// <returns>list features</returns>
        Task<IEnumerable<Feature>> GetFeaturesOfPermissionsByAccountId(int accountId);

        /// <summary>
        /// Get list features not belong permission by id of account
        /// </summary>
        /// <param name="accountId">id of account</param>
        /// <returns>list features</returns>
        Task<IEnumerable<Feature>> GetFeaturesNotBelongPermissionsByAccountId(int accountId);
    }
}
