using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;
using System.Linq;
using WebClient.Core.Helpers;

namespace WebClient.Repositories.Implements
{
    public class EmployeePermissionRepository : BaseRepository<PhanQuyen>, IEmployeePermissionRepository
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
            var dsIdQuyenHienTai = await this.GetIdPermissionsOfUser(userId);
            var dsIdQuyenInsert = ids.Except(dsIdQuyenHienTai);
            var dsIdQuyenDelete = dsIdQuyenHienTai.Except(ids);

            await this.SetPermissionListForUser(dsIdQuyenInsert, userId, handler);
            await this.DeletePermissionsForUser(dsIdQuyenDelete, userId, handler);
        }

        /// <summary>
        /// Set permission list for a user
        /// </summary>
        /// <param name="idsQuyen"></param>
        /// <param name="userId"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        private async Task SetPermissionListForUser(IEnumerable<int> idsQuyen, int userId, int handler)
        {
            try
            {
                //todo
            } catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete permission list for a user
        /// </summary>
        /// <param name="idsQuyen"></param>
        /// <param name="userId"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        private async Task DeletePermissionsForUser(IEnumerable<int> idsQuyen, int userId, int handler)
        {
            try
            {
                //todo
            } catch (Exception)
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
            var sql = @"Select IdQuyen From PhanQuyen Where DaXoa = 0 And IdTaiKhoan = @userId And IdQuyen is not NULL";
            var param = new
            {
                userId = userId
            };
            var dsQuyen = await this.dbContext.QueryAsync<int>(
                sql: sql,
                param: param,
                commandType: CommandType.Text);
            return dsQuyen;
        }

        /// <summary>
        /// Set feature list for a user
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public async Task SetFeaturesForEmployee(IEnumerable<int> idsInsert, IEnumerable<Feature> idsDelete, int idUser, int handle)
        {
            try
            {
                // insert ChucNang 
                //TODO
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}