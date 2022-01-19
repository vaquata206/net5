using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WebClient.Core.Helper;
using WebClient.Core.Helpers;
using WebClient.Repositories;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class EmployeePermissionService : IEmployeePermissionService
    {
        private readonly IUnitOfWork unitOfWork;

        public EmployeePermissionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task SetPermissionsForEmployee(IEnumerable<int> ids, int userId, int handler)
        {
            var account = await this.unitOfWork.AccountRepository.GetByIdAsync(userId);
            if (account == null)
            {
                throw new Exception("Tài khoản này không tồn tại");
            }
            else if (account.Quan_Tri == Constants.States.Actived.GetHashCode())
            {
                throw new Exception("Không thể cấp quyền với tài khoản này");
            }

            await this.unitOfWork.EmployeePermissionRepository.SetPermissionsForUser(ids, userId, handler);
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Get id of permissions that a user can accepts
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>permission ids</returns>
        public async Task<IEnumerable<int>> GetIdPermissionsOfUser(int userId)
        {
            return await this.unitOfWork.EmployeePermissionRepository.GetIdPermissionsOfUser(userId);
        }

        public async Task SetFeaturesForEmployee(IEnumerable<int> ids, int idEmployee, int idUser)
        {
            await this.unitOfWork.EmployeePermissionRepository.SetFeaturesForEmployee(ids, idEmployee, idUser);
            this.unitOfWork.Commit();
        }
    }
}