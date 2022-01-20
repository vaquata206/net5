using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.ViewModels;
using WebClient.Repositories;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class PermissionService : IPermissionService
    {
        private DateTime now;
        private readonly IUnitOfWork unitOfWork;
        public PermissionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            now = DateTime.Now;
        }

        /// <summary>
        /// Add a permission
        /// </summary>
        /// <param name="permissionVM">The permission</param>
        /// <param name="userId">User id</param>
        /// <returns>A Permission</returns>
        public async Task<Permission> SaveAsync(PermissionVM permissionVM, int userId)
        {
            Permission permission;
            if (permissionVM.Id.HasValue && permissionVM.Id.Value != 0)
            {
                // edit
                permission = await this.unitOfWork.PermissionRepository.GetByIdAsync(permissionVM.Id.Value);
                if (permission == null)
                {
                    throw new Exception("Quyền này không tồn tại");
                }

                permission.TenQuyen = permissionVM.TenQuyen;
                permission.DaXoa = Constants.TrangThai.ChuaXoa;

                await this.unitOfWork.PermissionRepository.UpdateAsync(permission);
            }
            else
            {
                // add
                permission = new Permission
                {
                    TenQuyen = permissionVM.TenQuyen,
                    DaXoa = Constants.TrangThai.ChuaXoa,
                };

                await this.unitOfWork.PermissionRepository.AddAsync(permission);
            }
            this.unitOfWork.Commit();

            return permission;
        }

        /// <summary>
        /// Get the permission by id
        /// </summary>
        /// <param name="permissionId">Id of permission</param>
        /// <returns>A Permission</returns>
        public async Task<Permission> GetByIdAsync(int permissionId)
        {
            return await this.unitOfWork.PermissionRepository.GetByIdAsync(permissionId);
        }

        /// <summary>
        /// Get list permission
        /// </summary>
        /// <returns>list permission</returns>
        public async Task<IEnumerable<Permission>> GetPermissions()
        {
            return await this.unitOfWork.PermissionRepository.GetPermissions();
        }

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permissionId">Id of permission</param>
        /// <param name="userId">User id</param>
        /// <returns>A task</returns>
        public async Task DeleteAsync(int permissionId, int userId)
        {
            // update permission with DaXoa = 1 by id of permission
            var permission = await this.unitOfWork.PermissionRepository.GetByIdAsync(permissionId, true);
            permission.DaXoa = Constants.TrangThai.DaXoa;
            await this.unitOfWork.PermissionRepository.UpdateAsync(permission);

            // update list permission features with DaXoa = 1 by id permission
            await this.unitOfWork.PermissionRepository.DeleteListPermissionFeaturesByIdPermission(permissionId);

            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Sets departments that the account is avaiabled work
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="departmentIds">Department ids</param>
        /// <param name="handlerId">Id of user who is performing this action</param>
        public async Task SetDepartmentsAsync(int accountId, int[] departmentIds, int handlerId)
        {
            var account = await this.unitOfWork.AccountRepository.GetByIdAsync(accountId);
            //if (account.Quan_Tri == Constants.States.Actived.GetHashCode())
            //{
            //    throw new Exception("Không thể thay đổi với tài khoản này");
            //}

            await this.unitOfWork.PermissionRepository.SetDepartmentsAsync(accountId, departmentIds, handlerId);
            this.unitOfWork.Commit();
        }
    }
}
