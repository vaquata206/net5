using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class PermissionFeatureRepository : BaseRepository<ChucNangQuyen>, IPermissionFeatureRepository
    {
        public PermissionFeatureRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// delete permission feature by id permission and id feature
        /// </summary>
        /// <param name="permission_Feature">infor permission feature</param>
        /// <returns>no reuturn</returns>
        public async Task DeleteAsync(ChucNangQuyen permission_Feature)
        {
            var query = @"DELETE FROM ChucNangQuyen WHERE IdQuyen = @idQuyen AND IdChucNang = @idChucNang";
            await this.dbContext.ExecuteAsync(query,
                param: new
                {
                    idQuyen = permission_Feature.IdQuyen,
                    idChucNang = permission_Feature.IdChucNang,
                },
                commandType: CommandType.Text);
        }

        /// <summary>
        /// get list permission features by id of permission
        /// </summary>
        /// <param name="permissionId">id of permission</param>
        /// <returns>list of permission features</returns>
        public async Task<IEnumerable<ChucNangQuyen>> GetListsByPermissionIdAsync(int permissionId)
        {
            var query = @"Select * From ChucNangQuyen Where IdQuyen = @idQuyen";
            var list = await this.dbContext.QueryAsync<ChucNangQuyen>(query,
                    param: new
                    {
                        idQuyen = permissionId,
                    },
                    commandType: CommandType.Text);

            return list;
        }
    }
}
