using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Update a permission
        /// </summary>
        /// <param name="permission">The permission</param>
        /// <returns>A task</returns>
        public async Task UpdatePermissionAsync(Permission permission)
        {
            try
            {
                var query = @"UPDATE Dm_Quyen SET TEN_QUYEN = :TenQuyen, GHI_CHU = :GhiChu WHERE Id_Quyen = :IdQuyen";

                await this.dbContext.ExecuteAsync(query, param: new
                {
                    TenQuyen = permission.Ten_Quyen,
                    GhiChu = permission.Ghi_Chu,
                    IdQuyen = permission.Id_Quyen
                }, commandType: System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get permission by id 
        /// </summary>
        /// <param name="permissionId">permission id</param>
        /// <returns>The permission</returns>
        public async Task<Permission> GetPermissionByIdAsync(int permissionId)
        {
            var query = @"SELECT Id_Quyen, Ma_Quyen, Ten_Quyen, Tinh_Trang, Ghi_Chu FROM DM_Quyen WHERE Id_Quyen = :id";

            var permission = await this.dbContext.QueryFirstOrDefaultAsync<Permission>(query, param: new
            {
                id = permissionId,
            }, commandType: System.Data.CommandType.Text);

            return permission;
        }

        /// <summary>
        /// Get list permission
        /// </summary>
        /// <returns>List permission</returns>
        public async Task<IEnumerable<Permission>> GetPermissions()
        {
            var query = @"SELECT Id_Quyen, Ma_Quyen, Ten_Quyen, Tinh_Trang, Ghi_Chu FROM DM_Quyen WHERE Tinh_Trang = 1";

            var permissions = await this.dbContext.QueryAsync<Permission>(query, commandType: System.Data.CommandType.Text);
            return permissions;
        }

        /// <summary>
        /// Delete list permission features by id permission and update permission with tinh_trang = 0 by id of permission
        /// </summary>
        /// <param name="permissionId">id of permission</param>
        /// <returns>no reuturn</returns>
        public async Task DeleteAsync(int permissionId)
        {
            try
            {
                var query = @"ADMIN_QUYEN.DELETE_QUYEN";
                var dyParam = new OracleDynamicParameters();

                dyParam.Add("P_ID_QUYEN", OracleDbType.Int64, ParameterDirection.Input, permissionId);
                await this.dbContext.ExecuteAsync(query, param: dyParam, commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sets departments that the account is avaiabled work
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="departmentIds">Department ids</param>
        /// <param name="handlerId">Id of user who is performing this action</param>
        public async Task SetDepartmentsAsync(int accountId, int[] departmentIds, int handlerId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, accountId);
            if (departmentIds != null && departmentIds.Length > 0)
            {
                dyParam.Add("p_id_dvs", OracleDbType.Varchar2, ParameterDirection.Input, string.Join(",", departmentIds));
            }
            else
            {
                dyParam.Add("p_id_dvs", OracleDbType.Varchar2, ParameterDirection.Input);
            }

            dyParam.Add("p_id_nv_khoitao", OracleDbType.Varchar2, ParameterDirection.Input, handlerId);

            await this.dbContext.ExecuteAsync(
                sql: "ADMIN_QUYEN.SETDEPARTMENTS",
                param: dyParam,
                commandType: CommandType.StoredProcedure);
        }
    }
}
