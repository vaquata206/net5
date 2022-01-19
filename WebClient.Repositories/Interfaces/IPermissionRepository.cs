using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        /// <summary>
        /// Get list permission
        /// </summary>
        /// <returns>List permission</returns>
        Task<IEnumerable<Permission>> GetPermissions();

        /// <summary>
        /// Delete list permission features by id permission and update permission with tinh_trang = 0 by id of permission
        /// </summary>
        /// <param name="permissionId">id of permission</param>
        /// <returns>no reuturn</returns>
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
