using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    public interface IPermissionService
    {
        /// <summary>
        /// Add a permission
        /// </summary>
        /// <param name="permission">The permission</param>
        /// <returns>A task</returns>
        Task<Permission> SaveAsync(PermissionVM permission);

        /// <summary>
        /// Get the permission by id
        /// </summary>
        /// <param name="permissionId">Id of permission</param>
        /// <returns>A Permission</returns>
        Task<Permission> GetByIdAsync(int permissionId);

        /// <summary>
        /// Get list permission
        /// </summary>
        /// <returns>list permission</returns>
        Task<IEnumerable<Permission>> GetPermissions();

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permissionId">Id of permission</param>
        /// <returns>A task</returns>
        Task DeleteAsync(int permissionId);

        /// <summary>
        /// Sets departments that the account is avaiabled work
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="departmentIds">Department ids</param>
        /// <param name="handlerId">Id of user who is performing this action</param>
        Task SetDepartmentsAsync(int accountId, int[] departmentIds, int handlerId);
    }
}
