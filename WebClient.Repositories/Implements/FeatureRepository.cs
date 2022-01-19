using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class FeatureRepository : BaseRepository<Feature>, IFeatureRepository
    {
        public FeatureRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets all featuress
        /// </summary>
        /// <returns>A list feature</returns>
        public async Task<IEnumerable<Feature>> GetAllAsync()
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.dbContext.QueryAsync<Feature>(
                "ADMIN_FEATURE.GET_ALL", 
                param: dyParam, 
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Get all features of the user
        /// </summary>
        /// <param name="userId">Employee id</param>
        /// <returns>A feature list</returns>
        public async Task<IEnumerable<Feature>> GetFeaturesUser(int userId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, userId);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.dbContext.QueryAsync<Feature>(
                sql: "ADMIN_FEATURE.GET_MENU_FEATURES", 
                param: dyParam,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Id of the feature that will be deleted
        /// </summary>
        /// <param name="featureId">Id of feature</param>
        /// <returns>A Task</returns>
        public async Task DeleteFeatureAsync(int featureId)
        {
            var query = "ADMIN_FEATURE.DELETE_FEATURE";
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("P_ID_CN", OracleDbType.Int64, ParameterDirection.Input, featureId);

            await this.dbContext.ExecuteAsync(sql: query, dyParam, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Get list feature of permission by id of account
        /// </summary>
        /// <param name="accountId">id of account</param>
        /// <returns>list features</returns>
        public async Task<IEnumerable<Feature>> GetFeaturesOfPermissionsByAccountId(int accountId)
        {
            var query = "ADMIN_FEATURE.FEATURE_PERMISSION_ACCOUNT";
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, accountId);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.dbContext.QueryAsync<Feature>(
                query,
                param: dyParam,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Get list features not belong permission by id of account
        /// </summary>
        /// <param name="accountId">id of account</param>
        /// <returns>list features</returns>
        public async Task<IEnumerable<Feature>> GetFeaturesNotBelongPermissionsByAccountId(int accountId)
        {
            var query = "ADMIN_FEATURE.FEATURE_NOTPERMISSION_ACCOUNT";
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, accountId);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.dbContext.QueryAsync<Feature>(
                query,
                param: dyParam,
                commandType: CommandType.StoredProcedure);
        }
    }
}
