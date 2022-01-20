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
        /// <param name="account">Account info</param>
        /// <returns>A feature list</returns>
        public IEnumerable<Menu> GetMenuAsync(AccountInfo account) {
            IEnumerable<Feature> list;
            if (account.IsKhachHang)
            {
                list = new List<Feature>();
            }
            else
            {
                if (account.IdVaiTro == 1)
                {
                    list = new List<Feature>();
                }
                else
                {
                    list = new List<Feature>();
                }
            }

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
        public async Task SaveFeatureAsync(FeatureVM featureVM, int handler)
        {
            try
            {
                if (featureVM.Id.HasValue && featureVM.Id.Value != 0)
                {
                    // Edit
                    var entity = await this.unitOfWork.FeatureRepository.GetByIdAsync(featureVM.Id.Value, false);
                    if (entity == null)
                    {
                        throw new Exception("Chức năng này không tồn tại");
                    }

                    entity.ActionName = featureVM.ActionName;
                    entity.ControllerName = featureVM.ControllerName;
                    entity.IdCha = featureVM.IdCha ?? 0;
                    entity.TenChucNang = featureVM.TenChucNang;
                    entity.MoTa = featureVM.Mota;
                    entity.ActionName = featureVM.ActionName;
                    entity.Url = featureVM.Url;
                    entity.HienThi = featureVM.HienThi ? 1 : 0;
                    entity.KichHoat = featureVM.KichHoat ? 1 : 0;
                    entity.NgayCapNhat = DateTime.Now;
                    entity.NguoiCapNhat = handler;
                    await this.unitOfWork.FeatureRepository.UpdateAsync(entity);
                }
                else
                {
                    var idCha = featureVM.IdCha ?? 0;
                    var thuTuCan = 0;
                    var dsChucNang = await this.GetAllAsync();
                    var chucNangCha = dsChucNang.Where(obj => obj.Id == idCha).FirstOrDefault();
                    if (chucNangCha != null)
                    {
                        thuTuCan = chucNangCha.Children == null ? 1 : chucNangCha.Children.OrderBy(x => x.ThuTu).Last().ThuTu + 1;
                    }
                    else
                    {
                        thuTuCan = dsChucNang.Count();
                    }

                    var entity = new Feature
                    {
                        TenChucNang = featureVM.TenChucNang,
                        MoTa = featureVM.Mota,
                        ThuTu = thuTuCan,
                        ControllerName = featureVM.ControllerName,
                        ActionName = featureVM.ActionName,
                        IdCha = featureVM.IdCha ?? 0,
                        Url = featureVM.Url,
                        DaXoa = false,
                        HienThi = featureVM.HienThi? Constants.States.Actived.GetHashCode() : Constants.States.Disabed.GetHashCode(),
                        KichHoat = featureVM.KichHoat ? Constants.States.Actived.GetHashCode() : Constants.States.Disabed.GetHashCode(),
                        NgayKhoiTao = DateTime.Now,
                        NguoiKhoiTao = handler

                    };
                    await this.unitOfWork.FeatureRepository.AddAsync(entity);
                }
                this.unitOfWork.Commit();
            }
            catch (Exception )
            {
                throw;
            }
        }

        /// <summary>
        /// Id of the feature that will be deleted
        /// </summary>
        /// <param name="featureId">Id of feature</param>
        /// <returns>A Task</returns>
        public async Task DeleteFeatureAsync(int featureId, int handler)
        {
            var feature = await this.unitOfWork.FeatureRepository.GetByIdAsync(featureId, false);
            if (feature == null)
            {
                throw new Exception("Chức năng này không tồn tại");
            }

            feature.DaXoa = true;
            feature.NgayCapNhat = DateTime.Now;
            feature.NguoiCapNhat = handler;
            await this.unitOfWork.FeatureRepository.UpdateAsync(feature);
            await this.unitOfWork.FeatureRepository.CapNhatThuTuChucNang(feature.IdCha, feature.ThuTu);
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
            var children = features.Where(x => x.IdCha == parentId).OrderBy(x => x.ThuTu);
            foreach (var i in children)
            {
                list = list.Append(new Menu
                {
                    Id = i.Id,
                    ParentId = (i.IdCha != 0) ? i.IdCha : 0,
                    Controler = i.ControllerName?.ToLower(),
                    Action = i.ActionName?.ToLower(),
                    Name = i.TenChucNang,
                    Show = i.HienThi == 1,
                    Url = i.Url,
                    Icon = i.Icon
                });

                list = list.Concat(this.FeaturesToArrayMenu(i.Id, features));
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
                var groupFeatures = list.GroupBy(x => x.IdCha);

                foreach (var gr in groupFeatures)
                {
                    var fr = list.FirstOrDefault(y => y.Id == gr.Key);
                    if (fr != null)
                    {
                        fr.Children = gr.OrderBy(y => y.ThuTu);
                    }
                };

                // Gets root features that has ID_CN_PR = 0
                list = groupFeatures.Where(x => x.Key == 0).SelectMany(x => x).OrderBy(x => x.ThuTu);
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
            var fp = Enumerable.Empty<Feature>();
            var fe = Enumerable.Empty<Feature>();
            var account = await this.unitOfWork.AccountRepository.GetByIdAsync(accountId);
            if (true)
            {
                fp = await this.unitOfWork.FeatureRepository.GetAllAsync();
            }
            else
            {
                fp = await this.unitOfWork.FeatureRepository.GetFeaturesOfPermissionsByAccountId(accountId);
                fe = await this.unitOfWork.FeatureRepository.GetFeaturesNotBelongPermissionsByAccountId(accountId);
            }

            // feature list belong the employee's permissions
            result = fp.Select(x => new TreeNode
            {
                Id = x.Id.ToString(),
                TypeNode = "P"
            });

            // feature list expand to the employee
            result = result.Concat(fe.Select(x => new TreeNode
            {
                Id = x.Id.ToString(),
                TypeNode = "E"
            }));

            return result;
        }
    }
}
