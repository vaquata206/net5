using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
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
            var query = @"SELECT * FROM ChucNang WHERE DaXoa = @daXoa";

            var feature = await this.dbContext.QueryAsync<Feature>(query, param: new
            {
                daXoa = Constants.TrangThai.ChuaXoa
            }, commandType: System.Data.CommandType.Text);

            return feature;
        }

        public async Task CapNhatThuTuChucNang(int idCha, int stt)
        {
            var query = @"UPDATE chucnang SET chucnang.thutu = chucnang.thutu - 1 WHERE 
                            chucnang.idcha = @idCha AND chucnang.thutu > @stt AND chucnang.daxoa = 0";

            await this.dbContext.ExecuteAsync(
               sql: query,
               param: new
               {
                   idCha = idCha,
                   stt = stt
               },
               commandType: CommandType.Text
           );
        }

        /// <summary>
        /// Id of the feature that will be deleted
        /// </summary>
        /// <param name="featureId">Id of feature</param>
        /// <returns>A Task</returns>
        public async Task DeleteFeatureAsync(int featureId)
        {
            /*
            var query = "ADMIN_FEATURE.DELETE_FEATURE";
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("P_ID_CN", OracleDbType.Int64, ParameterDirection.Input, featureId);

            await this.dbContext.ExecuteAsync(sql: query, dyParam, commandType: CommandType.StoredProcedure);*/
            // todo: cap nhat sau
        }

        /// <summary>
        /// Get list feature of permission by id of account
        /// </summary>
        /// <param name="accountId">id of account</param>
        /// <returns>list features</returns>
        public async Task<IEnumerable<Feature>> GetFeaturesOfPermissionsByAccountId(int accountId)
        {
            var sql = @"SELECT DISTINCT CN.* from ChucNang CN
                        JOIN ChucNangQuyen CNQ ON CN.Id = CNQ.IdChucNang
                        JOIN PhanQuyen PQ ON CNQ.IdQuyen = PQ.IdQuyen
                        WHERE PQ.DaXoa = @daXoa AND CN.DaXoa = @daXoa and PQ.IdTaiKhoan = @acountId
						ORDER BY ThuTu";
            return await this.dbContext.QueryAsync<Feature>(
                sql: sql,
                param: new
                {
                    acountId = accountId,
                    daXoa = Constants.TrangThai.ChuaXoa
                },
                commandType: CommandType.Text
                );
        }

        /// <summary>
        /// Get list features not belong permission by id of account
        /// </summary>
        /// <param name="accountId">id of account</param>
        /// <returns>list features</returns>
        public async Task<IEnumerable<Feature>> GetFeaturesNotBelongPermissionsByAccountId(int accountId)
        {
            var sql = @"SELECT DISTINCT CN.* from ChucNang CN
                        JOIN PhanQuyen PQ ON CN.Id = PQ.IdChucNang
                        WHERE PQ.DaXoa = @daXoa AND CN.DaXoa = @daXoa and PQ.IdTaiKhoan = @acountId
						ORDER BY ThuTu";
            return await this.dbContext.QueryAsync<Feature>(
                sql: sql,
                param: new
                {
                    acountId = accountId,
                    daXoa = Constants.TrangThai.ChuaXoa
                },
                commandType: CommandType.Text
                );
        }
    }
}
