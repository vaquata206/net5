using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    public interface IEmployeeService
    {
        /// <summary>
        /// get employee view model by id nhanvien
        /// </summary>
        /// <param name="employeeId">employee's id</param>
        /// <returns>the employee instance</returns>
        Task<Employee> GetByIdAsync(int employeeId);

        /// <summary>
        /// update profile
        /// </summary>
        /// <param name="employeeVM">the employee VM</param>
        /// <param name="userId">user id</param>
        /// <returns>A task</returns>
        Task UpdateProfileAsync(EmployeeVM employeeVM, int userId);

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

        /// <summary>
        /// delete employee by id
        /// </summary>
        /// <param name="employeeId">employee id</param>
        /// <param name="userId">Current user id</param>
        /// <returns>the Task</returns>
        Task DeleteByIdAsync(int employeeId, int userId);

        /// <summary>
        /// save employee (insert or update)
        /// </summary>
        /// <param name="employeeVM">the employee VM</param>
        /// <param name="userId">the current userid</param>
        /// <returns>the task</returns>
        Task SaveAsync(EmployeeVM employeeVM, int userId);
    }
}
