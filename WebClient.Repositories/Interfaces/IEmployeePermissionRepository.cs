using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IEmployeePermissionRepository : IBaseRepository<Employee_Permission>
    {
        Task SetPermissionsForUser(IEnumerable<int> ids, int idEmployee, int idUser);

        /// <summary>
        /// Get id of permissions that a user can accepts
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>permission ids</returns>
        Task<IEnumerable<int>> GetIdPermissionsOfUser(int userId);
        Task SetFeaturesForEmployee(IEnumerable<int> ids, int idEmployee, int idUser);
    }
}