using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.ViewModels;
using WebClient.Repositories;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// get employee view model by employee id
        /// </summary>
        /// <param name="employeeId">employee's id</param>
        /// <returns>the employee instance</returns>
        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            return await this.unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
        }

        /// <summary>
        /// update profile
        /// </summary>
        /// <param name="employeeVM">the employee VM</param>
        /// <param name="userId">user id</param>
        /// <returns>A task</returns>
        public async Task UpdateProfileAsync(EmployeeVM employeeVM, int userId)
        {
            await this.unitOfWork.EmployeeRepository.UpdateProfileAsync(employeeVM, userId);
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Get employees by department id
        /// </summary>
        /// <param name="deparmentId">Id of deparment</param>
        /// <returns>List employee</returns>
        public async Task<IEnumerable<Employee>> GetEmployeesByDeparmentIdAsync(int deparmentId)
        {
            return await this.unitOfWork.EmployeeRepository.GetEmployeesByDeparmentIdAsync(deparmentId);
        }

        /// <summary>
        /// Get employees by chức vụ
        /// </summary>
        /// <param name="chucVu">Chức vụ</param>
        /// <returns>List employee</returns>
        public async Task<IEnumerable<Employee>> GetEmployeesByChucVuAsync(int chucVu)
        {
            return await this.unitOfWork.EmployeeRepository.GetEmployeesByChucVuAsync(chucVu);
        }

        /// <summary>
        /// delete employee by id
        /// </summary>
        /// <param name="employeeId">employee id</param>
        /// <param name="userId">Current user id</param>
        /// <returns>the Task</returns>
        public async Task DeleteByIdAsync(int employeeId, int userId)
        {
            var employee = await this.unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
            employee.Id_NV_CapNhat = userId;
            employee.Ngay_CapNhat = DateTime.Now;
            employee.Tinh_Trang = Constants.States.Disabed.GetHashCode();
            await this.unitOfWork.EmployeeRepository.UpdateAsync(employee);
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// save employee (insert or update)
        /// </summary>
        /// <param name="employeeVM">the employee VM</param>
        /// <param name="userId">the current userid</param>
        /// <returns>the task</returns>
        public async Task SaveAsync(EmployeeVM employeeVM, int userId)
        {
            Employee emp = this.mapper.Map<Employee>(employeeVM);
            var department = await this.unitOfWork.DepartmentRepository.GetByIdAsync(emp.Id_DonVi);
            if (department == null)
            {
                throw new Exception("Đơn vị không đúng");
            }

            emp.Tinh_Trang = Constants.States.Actived.GetHashCode();
            if (employeeVM.Id_NhanVien > 0)
            {
                var oldEmp = await this.unitOfWork.EmployeeRepository.GetByIdAsync(employeeVM.Id_NhanVien);
                emp.Ma_NhanVien = oldEmp.Ma_NhanVien;
                emp.Id_NV_CapNhat = userId;
                emp.Ngay_CapNhat = DateTime.Now;
                emp.Id_NV_KhoiTao = oldEmp.Id_NV_KhoiTao;
                emp.Ngay_KhoiTao = oldEmp.Ngay_KhoiTao;

                await this.unitOfWork.EmployeeRepository.UpdateAsync(emp);
            }
            else
            {
                emp.Ma_NhanVien = "NV" + DateTime.Now.Ticks;
                emp.Id_NV_KhoiTao = userId;
                emp.Ngay_KhoiTao = DateTime.Now;
                await this.unitOfWork.EmployeeRepository.AddAsync(emp);
            }

            this.unitOfWork.Commit();
        }
    }
}
