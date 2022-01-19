using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.Models;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="username">The usename</param>
        /// <param name="password">The password</param>
        /// <returns>Access token</returns>
        public async Task<AccountInfo> LoginAsync(string username, string password)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_username", OracleDbType.Varchar2, ParameterDirection.Input, username);
            dyParam.Add("p_password", OracleDbType.Varchar2, ParameterDirection.Input, Common.ComputeSha256Hash(password));
            dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.dbContext.QueryFirstOrDefaultAsync<AccountInfo>(
                sql: "BLDT_ACCOUNT.LOGIN",
                param: dyParam,
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task<bool> ChangePasswordAsync(string userName, int id_NguoiDung, string matKhauCu, string matKhauMoi)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_username", OracleDbType.Varchar2, ParameterDirection.Input, userName);
            dyParam.Add("p_id_nhanvien", OracleDbType.Int64, ParameterDirection.Input, id_NguoiDung);
            dyParam.Add("p_current_password", OracleDbType.Varchar2, ParameterDirection.Input, Common.ComputeSha256Hash(matKhauCu));
            dyParam.Add("p_new_password", OracleDbType.Varchar2, ParameterDirection.Input, Common.ComputeSha256Hash(matKhauMoi));
            dyParam.Add("rs", OracleDbType.Int16, ParameterDirection.Output);
            await this.dbContext.QueryFirstOrDefaultAsync<int>(
                sql: "BLDT_ACCOUNT.ChangePassword", 
                param: dyParam, 
                commandType: CommandType.StoredProcedure);

            var status = int.Parse(dyParam.GetByName("rs").Value.ToString());
            return status == Constants.States.Actived.GetHashCode();
        }

        /// <summary>
        /// Gets employee's accounts
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <returns>List account</returns>
        public async Task<IEnumerable<Account>> GetAccountsByEmployeeId(int id)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_employeeId", OracleDbType.Int64, ParameterDirection.Input, id);
            dyParam.Add("rs", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.dbContext.QueryAsync<Account>(
                sql: "BLDT_ACCOUNT.GETACCOUNTSBYEMPLOYEEID", 
                dyParam, 
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// get account by username
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns>account</returns>
        public async Task<Account> GetAccountByUsername(string userName)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_username", OracleDbType.Varchar2, ParameterDirection.Input, userName);
            dyParam.Add("rs", OracleDbType.RefCursor, ParameterDirection.Output);
            var query = "BLDT_ACCOUNT.GETACCOUNTBYUSERNAME";
            return await this.dbContext.QueryFirstOrDefaultAsync<Account>(
                sql: query, param: dyParam, commandType: CommandType.StoredProcedure);
        }
    }
}
