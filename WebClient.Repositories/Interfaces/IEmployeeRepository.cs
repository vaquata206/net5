using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;

namespace WebClient.Repositories.Interfaces
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        /// <summary>
        /// Update information's Employee
        /// </summary>
        /// <param name="employee">the employee</param>
        /// <returns>the employee after updated</returns>
        Task<Employee> UpdateProfileAsync(EmployeeVM employee, int userId);

        /// <summary>
        /// Get employees by department id
        /// </summary>
        /// <param name="deparmentId">Id of deparment</param>
        /// <returns>List employee</returns>
        Task<IEnumerable<Employee>> GetEmployeesByDeparmentIdAsync(int deparmentId);

        /// <summary>
        /// Get employees by chức vụ
        /// </summary>
        /// <param name="chucVu">Chức vụ</param>
        /// <returns>List employee</returns>
        Task<IEnumerable<Employee>> GetEmployeesByChucVuAsync(int chucVu);
    }
}
