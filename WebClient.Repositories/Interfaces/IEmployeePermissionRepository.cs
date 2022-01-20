using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IEmployeePermissionRepository : IBaseRepository<PhanQuyen>
    {
        Task SetPermissionsForUser(IEnumerable<int> ids, int idEmployee, int idUser);

        /// <summary>
        /// Get id of permissions that a user can accepts
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>permission ids</returns>
        Task<IEnumerable<int>> GetIdPermissionsOfUser(int userId);

        /// <summary>
        /// Set feature list for a user
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        Task SetFeaturesForEmployee(IEnumerable<int> idsInsert, IEnumerable<Feature> idsDelete, int idUser, int handle);
    }
}