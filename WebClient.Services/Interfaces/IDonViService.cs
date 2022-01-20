using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    public interface IDonViService
    {
        /// <summary>
        /// Get departments with condition: to idDepartmentStart from idDepartmentEnd.
        /// </summary>
        /// <param name="userId">Account Id whose user is performing this action</param>
        /// <param name="departmentId">id department start</param>
        /// <param name="idDepartmentEnd">id department End. Default = -1, if idDepartmentEnd = -1 then return all.</param>
        /// <returns>list departments </returns>
        Task<IEnumerable<DonVi>> GetTreeDepartmentsdWithTerm(int userId, int departmentId, int idDepartmentEnd = -1);

        /// <summary>
        /// Get tree nodes that are children of the department
        /// </summary>
        /// <param name="departmentId">Id of department</param>
        /// <returns>Tree nodes</returns>
        Task<IEnumerable<TreeNode>> GetTreeNodeChildrenOfDepartment(int departmentId, int accountId);

        /// <summary>
        /// Id of the Department that will be deleted
        /// </summary>
        /// <param name="idDonVi">Id of Department</param>
        /// <param name="userId">Who is doing this action</param>
        /// <returns>a Task</returns>
        Task DeleteAsync(int idDonVi, int userId);

        /// <summary>
        /// Save a Department
        /// </summary>
        /// <param name="department">The Department</param>
        /// <param name="userId">Who is doing this action</param>
        /// <returns>A task</returns>
        Task SaveAsync(DepartmentVM department, int userId);

        /// <summary>
        /// Get department by id
        /// </summary
        /// <param name="id">The id of department</param>
        /// <returns>The department</returns>
        Task<DonVi> GetByIdAsync(int id);

        Task<IEnumerable<DonVi>> GetDepartmentsByIdParent(int idParent, int accountId);
    }
}
