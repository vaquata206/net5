using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;
using WebClient.Repositories;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class FeatureService : IFeatureService
    {
        private readonly IUnitOfWork unitOfWork;
        public FeatureService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get menu of a account
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>A feature list</returns>
        public async Task<IEnumerable<Menu>> GetMenuAsync(int userId) {
            var list = await this.unitOfWork.FeatureRepository.GetFeaturesUser(userId);
            return this.FeaturesToArrayMenu(0, list);
        }

        /// <summary>
        /// Gets all featuress
        /// </summary>
        /// <returns>A list feature</returns>
        public async Task<IEnumerable<Feature>> GetAllAsync()
        {
            var list = await this.unitOfWork.FeatureRepository.GetAllAsync();
            list = this.ConvertToTree(list);
            return list;
        }

        /// <summary>
        /// Save a feature
        /// </summary>
        /// <param name="featureVM">The feature</param>
        /// <returns>A task</returns>
        public async Task SaveFeatureAsync(FeatureVM featureVM)
        {
            try
            {
                if (featureVM.ID_CN.HasValue && featureVM.ID_CN.Value != 0)
                {
                    // Edit
                    var entity = await this.unitOfWork.FeatureRepository.GetByIdAsync(featureVM.ID_CN.Value, false);
                    if (entity == null)
                    {
                        throw new Exception("Chức năng này không tồn tại");
                    }

                    entity.Action_Name = featureVM.Action_Name;
                    entity.Controller_Name = featureVM.Controller_Name;
                    entity.Id_ChucNang_Cha = featureVM.ID_CN_PR ?? 0;
                    entity.Ten_ChucNang = featureVM.Ten_CN;
                    entity.Tooltip = featureVM.ToolTip_CN;
                    entity.MoTa_ChucNang = featureVM.Mota_CN;
                    entity.Action_Name = featureVM.Action_Name;
                    entity.HienThi_Menu = featureVM.HienThi_Menu ? 1 : 0;

                    await this.unitOfWork.FeatureRepository.UpdateAsync(entity);
                }
                else
                {
                    var idCha = featureVM.ID_CN_PR ?? 0;
                    var thuTuCan = 0;
                    var dsChucNang = await this.GetAllAsync();
                    var chucNangCha = dsChucNang.Where(obj => obj.Id_ChucNang == idCha).FirstOrDefault();
                    if (chucNangCha != null)
                    {
                        thuTuCan = chucNangCha.Children == null ? 1 : chucNangCha.Children.OrderBy(x => x.Thu_Tu).Last().Thu_Tu + 1;
                    }
                    else
                    {
                        thuTuCan = dsChucNang.Count();
                    }

                    var entity = new Feature
                    {
                        Ten_ChucNang = featureVM.Ten_CN,
                        Ma_ChucNang = "CN" + string.Format("{0:yyyyMMddhhmmss}", DateTime.Now),
                        Tooltip = featureVM.ToolTip_CN,
                        MoTa_ChucNang = featureVM.Mota_CN,
                        Controller_Name = featureVM.Controller_Name,
                        Action_Name = featureVM.Action_Name,
                        Id_ChucNang_Cha = featureVM.ID_CN_PR ?? 0,
                        Thu_Tu = thuTuCan,
                        Tinh_Trang = 1,
                        Id_ChuongTrinh = 1,
                        HienThi_Menu = featureVM.HienThi_Menu ? Constants.States.Actived.GetHashCode() : Constants.States.Disabed.GetHashCode()
                    };
                    await this.unitOfWork.FeatureRepository.AddAsync(entity);
                }
                this.unitOfWork.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Id of the feature that will be deleted
        /// </summary>
        /// <param name="featureId">Id of feature</param>
        /// <returns>A Task</returns>
        public async Task DeleteFeatureAsync(int featureId)
        {
            var feature = await this.unitOfWork.FeatureRepository.GetByIdAsync(featureId, false);
            if (feature == null)
            {
                throw new Exception("Chức năng này không tồn tại");
            }

            await this.unitOfWork.FeatureRepository.DeleteFeatureAsync(featureId);
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Convert a list features to array menus
        /// </summary>
        /// <param name="parentId">Parent id</param>
        /// <param name="features">The features</param>
        /// <returns>A menu list</returns>
        private IEnumerable<Menu> FeaturesToArrayMenu(int parentId, IEnumerable<Feature> features)
        {
            var list = Enumerable.Empty<Menu>();
            var children = features.Where(x => x.Id_ChucNang_Cha == parentId).OrderBy(x => x.Thu_Tu);
            foreach (var i in children)
            {
                list = list.Append(new Menu
                {
                    Id = i.Id_ChucNang,
                    ParentId = (i.Id_ChucNang_Cha != 0) ? i.Id_ChucNang_Cha : 0,
                    Controler = i.Controller_Name?.ToLower(),
                    Action = i.Action_Name?.ToLower(),
                    Name = i.Ten_ChucNang,
                    Show = i.HienThi_Menu == 1,
                    Icon = i.Icon
                });

                list = list.Concat(this.FeaturesToArrayMenu(i.Id_ChucNang, features));
            }

            return list;
        }

        /// <summary>
        /// Convert a list to tree
        /// </summary>
        /// <param name="list">features</param>
        /// <returns>A tree list</returns>
        private IEnumerable<Feature> ConvertToTree(IEnumerable<Feature> list)
        {
            if (list != null && list.Any())
            {
                // groups features by Id feature parent
                var groupFeatures = list.GroupBy(x => x.Id_ChucNang_Cha);

                foreach (var gr in groupFeatures)
                {
                    var fr = list.FirstOrDefault(y => y.Id_ChucNang == gr.Key);
                    if (fr != null)
                    {
                        fr.Children = gr.OrderBy(y => y.Thu_Tu);
                    }
                };

                // Gets root features that has ID_CN_PR = 0
                list = groupFeatures.Where(x => x.Key == 0).SelectMany(x => x).OrderBy(x => x.Thu_Tu);
            }

            return list;
        }

        /// <summary>
        /// get tree node features by id of account
        /// </summary>
        /// <param name="accountId">id of account</param>
        /// <returns>list tree node features</returns>
        public async Task<IEnumerable<TreeNode>> GetTreeNodeFeaturesOfAccount(int accountId)
        {
            var result = Enumerable.Empty<TreeNode>();
            var fp = await this.unitOfWork.FeatureRepository.GetFeaturesOfPermissionsByAccountId(accountId);
            var fe = await this.unitOfWork.FeatureRepository.GetFeaturesNotBelongPermissionsByAccountId(accountId);

            // feature list belong the employee's permissions
            result = fp.Select(x => new TreeNode
            {
                Id = x.Id_ChucNang.ToString(),
                TypeNode = "P"
            });

            // feature list expand to the employee
            result = result.Concat(fe.Select(x => new TreeNode
            {
                Id = x.Id_ChucNang.ToString(),
                TypeNode = "E"
            }));

            return result;
        }
    }
}
