using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class EmployeePermissionRepository : BaseRepository<Employee_Permission>, IEmployeePermissionRepository
    {
        public EmployeePermissionRepository(DbContext dbContext) : base(dbContext)
        {
        }
        /// <summary>
        /// Set permission for a user
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userId"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public async Task SetPermissionsForUser(IEnumerable<int> ids, int userId, int handler)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("P_IDS", OracleDbType.Varchar2, ParameterDirection.Input, string.Join(",", ids));
                dyParam.Add("P_ID_ND", OracleDbType.Int64, ParameterDirection.Input, userId);
                dyParam.Add("P_ID_NV_TT", OracleDbType.Int64, ParameterDirection.Input, handler);
                await this.dbContext.ExecuteAsync(
                    sql: "ADMIN_NHANVIEN_QUYEN.SET_PERMISSIONS",
                    param: dyParam,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get id of permissions that a user can accepts
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>permission ids</returns>
        public async Task<IEnumerable<int>> GetIdPermissionsOfUser(int userId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("P_ID_ND", OracleDbType.Int64, ParameterDirection.Input, userId);
            dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.dbContext.QueryAsync<int>(
                sql: "ADMIN_NHANVIEN_QUYEN.GET_PERMISSIONIDS_OF_USER",
                param: dyParam,
                commandType: CommandType.StoredProcedure);
        }

        public async Task SetFeaturesForEmployee(IEnumerable<int> ids, int idEmployee, int idUser)
        {
            try
            {
                var query = "ADMIN_NHANVIEN_QUYEN.SET_FEATURES";
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("P_ID_CNS", OracleDbType.Varchar2, ParameterDirection.Input, string.Join(",", ids));
                dyParam.Add("P_ID_NV", OracleDbType.Int64, ParameterDirection.Input, idEmployee);
                dyParam.Add("P_ID_USER", OracleDbType.Int64, ParameterDirection.Input, idUser);
                await this.dbContext.ExecuteAsync(query, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
