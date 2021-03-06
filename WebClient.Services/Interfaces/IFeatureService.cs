using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    /// <summary>
    /// Feature service
    /// </summary>
    public interface IFeatureService
    {
        /// <summary>
        /// Get menu of a account
        /// </summary>
        /// <param name="account">Account info</param>
        /// <returns>A feature list</returns>
        IEnumerable<Menu> GetMenuAsync(AccountInfo account);

        /// <summary>
        /// Gets all featuress
        /// </summary>
        /// <returns>A list feature</returns>
        Task<IEnumerable<Feature>> GetAllAsync();

        /// <summary>
        /// Save a feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <returns>A task</returns>
        Task SaveFeatureAsync(FeatureVM feature, int handler);

        /// <summary>
        /// Id of the feature that will be deleted
        /// </summary>
        /// <param name="featureId">Id of feature</param>
        /// <returns>A Task</returns>
        Task DeleteFeatureAsync(int featureId, int handler);

        /// <summary>
        /// get tree node features by id of account
        /// </summary>
        /// <param name="accountId">id of account</param>
        /// <returns>list tree node features</returns>
        Task<IEnumerable<TreeNode>> GetTreeNodeFeaturesOfAccount(int accountId);
    }
}
