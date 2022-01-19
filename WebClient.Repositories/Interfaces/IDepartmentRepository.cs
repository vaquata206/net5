using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        /// <summary>
        /// Get departments with condition: to idDepartmentStart from idDepartmentEnd.
        /// </summary>
        /// <param name="idDepartment">id department start</param>
        /// <param name="accountId">Account Id whose user is performing this action</param>
        /// <param name="idDepartmentEnd">id department End, if idDepartmentEnd = -1 then return all.</param>
        /// <returns>list departments </returns>
        Task<IEnumerable<Department>> GetDepartmentsWithTerm(int accountId, int idDepartment, int idDepartmentEnd = -1);

        /// <summary>
        /// Gets children that are controlled by the account by parent Id
        /// </summary>
        /// <param name="parentId">parent department id</param>
        /// <param name="accountId">Account id</param>
        /// <returns>List department</returns>
        Task<IEnumerable<Department>> GetDepartmentsByIdParent(int idParent, int accountId);
    }
}
