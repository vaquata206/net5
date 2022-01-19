using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.Services.Interfaces
{
    public interface IEmployeePermissionService
    {
        Task SetPermissionsForEmployee(IEnumerable<int> ids, int idEmployee, int idUser);

        /// <summary>
        /// Get id of permissions that a user can accepts
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>permission ids</returns>
        Task<IEnumerable<int>> GetIdPermissionsOfUser(int userId);
        Task SetFeaturesForEmployee(IEnumerable<int> ids, int idEmployee, int idUser);
    }
}