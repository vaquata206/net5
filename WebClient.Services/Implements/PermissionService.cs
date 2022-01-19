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
        private readonly IUnitOfWork unitOfWork;
        public PermissionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Add a permission
        /// </summary>
        /// <param name="permissionVM">The permission</param>
        /// <returns>A Permission</returns>
        public async Task<Permission> SaveAsync(PermissionVM permissionVM)
        {
            Permission permission;
            if (permissionVM.Id_Quyen.HasValue && permissionVM.Id_Quyen.Value != 0)
            {
                // edit
                permission = await this.unitOfWork.PermissionRepository.GetByIdAsync(permissionVM.Id_Quyen.Value);
                if (permission == null)
                {
                    throw new Exception("Quyền này không tồn tại");
                }

                permission.Ten_Quyen = permissionVM.Ten_Quyen;
                permission.Ghi_Chu = permissionVM.Ghi_Chu;

                await this.unitOfWork.PermissionRepository.UpdateAsync(permission);
            }
            else
            {
                // add
                permission = new Permission
                {
                    Ma_Quyen = "MQ" + string.Format("{0:yyyyMMddhhmmss}", DateTime.Now),
                    Ghi_Chu = permissionVM.Ghi_Chu,
                    Ten_Quyen = permissionVM.Ten_Quyen,
                    Tinh_Trang = Constants.States.Actived.GetHashCode()
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
        /// <returns>A task</returns>
        public async Task DeleteAsync(int permissionId)
        {
            await this.unitOfWork.PermissionRepository.DeleteAsync(permissionId);
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
            if (account.Quan_Tri == Constants.States.Actived.GetHashCode())
            {
                throw new Exception("Không thể thay đổi với tài khoản này");
            }

            await this.unitOfWork.PermissionRepository.SetDepartmentsAsync(accountId, departmentIds, handlerId);
            this.unitOfWork.Commit();
        }
    }
}
