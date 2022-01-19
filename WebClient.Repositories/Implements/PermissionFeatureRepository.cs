using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class PermissionFeatureRepository : BaseRepository<Permission_Feature>, IPermissionFeatureRepository
    {
        public PermissionFeatureRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// delete permission feature by id permission and id feature
        /// </summary>
        /// <param name="permission_Feature">infor permission feature</param>
        /// <returns>no reuturn</returns>
        public async Task DeleteAsync(Permission_Feature permission_Feature)
        {
            var query = @"DELETE FROM Quyen_ChucNang WHERE Id_Quyen = :idQuyen AND Id_ChucNang = :idChucNang";
            await this.dbContext.ExecuteAsync(query,
                param: new
                {
                    idQuyen = permission_Feature.Id_Quyen,
                    idChucNang = permission_Feature.Id_ChucNang,
                },
                commandType: CommandType.Text);
        }

        /// <summary>
        /// get list permission features by id of permission
        /// </summary>
        /// <param name="permissionId">id of permission</param>
        /// <returns>list of permission features</returns>
        public async Task<IEnumerable<Permission_Feature>> GetListsByPermissionIdAsync(int permissionId)
        {
            var query = @"Select * From quyen_chucnang Where Id_Quyen = :idQuyen";
            var list = await this.dbContext.QueryAsync<Permission_Feature>(query,
                    param: new
                    {
                        idQuyen = permissionId,
                    },
                    commandType: CommandType.Text);

            return list;
        }
    }
}
