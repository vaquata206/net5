using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class DonViRepository : BaseRepository<DonVi>, IDonViRepository
    {
        public DonViRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Get departments with condition: to idDepartmentStart from idDepartmentEnd.
        /// </summary>
        /// <param name="idDepartment">id department start</param>
        /// <param name="accountId">Account Id whose user is performing this action</param>
        /// <param name="idDepartmentEnd">id department End, if idDepartmentEnd = -1 then return all.</param>
        /// <returns>list departments </returns>
        public async Task<IEnumerable<DonVi>> GetDepartmentsdWithTerm(int userId, int departmentId, int idDepartmentEnd = -1)
        {
            string sql = "select dvi.* from donvi dvi where daXoa = 0";
            return await this.dbContext.QueryAsync<DonVi>(
                sql: sql,
                param: new {departmentId},
                commandType: CommandType.Text
                );
        }

        /// <summary>
        /// Gets children that are controlled by the account by parent Id
        /// </summary>
        /// <param name="parentId">parent department id</param>
        /// <param name="accountId">Account id</param>
        /// <returns>List department</returns>
        public async Task<IEnumerable<DonVi>> GetDepartmentsByIdParent(int idParent, int accountId)
        {
            var dsDonViSql = @"Select * From DonVi Where DonViCha = @donViCha and DaXoa = 0";
            var paramDv = new
            {
                donViCha = idParent
            };
            var dsDonVi = await this.dbContext.QueryAsync<DonVi>(
                sql: dsDonViSql,
                param: paramDv,
                commandType: CommandType.Text);
            return dsDonVi;
        }
    }
}
