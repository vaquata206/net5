using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Repositories;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class PermissionFeatureService : IPermissionFeatureService
    {
        private readonly IUnitOfWork unitOfWork;

        public PermissionFeatureService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// set new list features for permission
        /// </summary>
        /// <param name="featureIds">list id of features</param>
        /// <param name="permissionId">id of permission</param>
        /// <param name="idNhanVien">id of handler</param>
        /// <returns>no return</returns>
        public async Task SetFeaturesForPermissionAsync(IEnumerable<int> featureIds, int permissionId, int idNhanVien)
        {
            var oldFeatures = await this.unitOfWork.PermissionFeatureRepository.GetListsByPermissionIdAsync(permissionId);

            var currentDate = DateTime.Now;
            var newFeatures = featureIds.Where(x => oldFeatures.All(y => y.Id_ChucNang != x)).Select(x => new Permission_Feature {
                Id_ChucNang = x,
                Id_Quyen = permissionId,
                Ngay_KhoiTao = currentDate,
                Id_NV_KhoiTao = idNhanVien
            });
            var removedFeatures = oldFeatures.Where(x => featureIds.All(y => y != x.Id_ChucNang));

            foreach(var i in removedFeatures)
            {
                await this.unitOfWork.PermissionFeatureRepository.DeleteAsync(i);
            }

            foreach(var i in newFeatures)
            {
                await this.unitOfWork.PermissionFeatureRepository.AddAsync(i);
            }

            this.unitOfWork.Commit();
        }

        /// <summary>
        /// get: permission and features by permission id
        /// </summary>
        /// <param name="permissionId">id of permission</param>
        /// <returns>list of permission features</returns>
        public async Task<IEnumerable<Permission_Feature>> GetPermissionFeaturesByPermissionId(int permissionId)
        {
            return await this.unitOfWork.PermissionFeatureRepository.GetListsByPermissionIdAsync(permissionId);
        }
    }
}
