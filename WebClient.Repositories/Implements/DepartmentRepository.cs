using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Get departments with condition: to idDepartmentStart from idDepartmentEnd.
        /// </summary>
        /// <param name="idDepartment">id department start</param>
        /// <param name="accountId">Account Id whose user is performing this action</param>
        /// <param name="idDepartmentEnd">id department End, if idDepartmentEnd = -1 then return all.</param>
        /// <returns>list departments </returns>
        public async Task<IEnumerable<Department>> GetDepartmentsWithTerm(int accountId, int idDepartment, int idDepartmentEnd = -1)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_donvi", OracleDbType.Int64, ParameterDirection.Input, idDepartment);
            dyParam.Add("p_end", OracleDbType.Int64, ParameterDirection.Input, idDepartmentEnd);
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, accountId);
            dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.dbContext.QueryAsync<Department>(
                sql: "BLDT_DONVI.GET_DEPARTMENTS_WITH_TERM",
                param: dyParam,
                commandType: CommandType.StoredProcedure);

        }

        /// <summary>
        /// Gets children that are controlled by the account by parent Id
        /// </summary>
        /// <param name="parentId">parent department id</param>
        /// <param name="accountId">Account id</param>
        /// <returns>List department</returns>
        public async Task<IEnumerable<Department>> GetDepartmentsByIdParent(int idParent, int accountId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_dv_cha", OracleDbType.Int64, ParameterDirection.Input, idParent);
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, accountId);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.dbContext.QueryAsync<Department>(
                sql: "BLDT_DONVI.GETDEPARTMENTS",
                param: dyParam,
                commandType: CommandType.StoredProcedure);
        }
    }
}
