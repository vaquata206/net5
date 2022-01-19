using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.ViewModels;
using WebClient.Repositories;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class DepartmentService: IDepartmentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DepartmentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        /// <summary>
        /// Get tree nodes that are children of the department
        /// </summary>
        /// <param name="departmentId">Id of department</param>
        /// <returns>Tree nodes</returns>
        public async Task<IEnumerable<TreeNode>> GetTreeNodeChildrenOfDepartment(int departmentId, int accountId)
        {
            var result = Enumerable.Empty<TreeNode>();
            var departments = await this.unitOfWork.DepartmentRepository.GetDepartmentsByIdParent(departmentId, accountId);
            result = departments.Select(x => new TreeNode
            {
                Id = "D" + x.Id_DonVi,
                Children = true,
                Text = x.Ten_DonVi,
                TypeNode = "Deparment"
            });

            var employees = await this.unitOfWork.EmployeeRepository.GetEmployeesByDeparmentIdAsync(departmentId);

            result = result.Concat(employees.Select(x => new TreeNode
            {
                Id = "E" + x.Id_NhanVien,
                Children = true,
                Text = x.Ho_Ten,
                TypeNode = "Employee"
            }));

            return result;
        }

        /// <summary>
        /// Get departments with condition: to idDepartmentStart from idDepartmentEnd.
        /// </summary>
        /// <param name="userId">Account Id whose user is performing this action</param>
        /// <param name="departmentId">id department start</param>
        /// <param name="idDepartmentEnd">id department End. Default = -1, if idDepartmentEnd = -1 then return all.</param>
        /// <returns>list departments </returns>
        public async Task<IEnumerable<Department>> GetTreeDepartmentsdWithTerm(int userId, int departmentId, int idDepartmentEnd = -1)
        {
            var departments = await this.unitOfWork.DepartmentRepository.GetDepartmentsWithTerm(userId, departmentId, idDepartmentEnd);
            return this.ConvertToTree(departments, departmentId);
        }

        /// <summary>
        /// Id of the Department that will be deleted
        /// </summary>
        /// <param name="iddonvi">Id of Department</param>
        /// <param name="handler">Who is doing this action</param>
        /// <returns>a Task</returns>
        public async Task DeleteAsync(int idDonVi, int userId)
        {
            var department = await this.unitOfWork.DepartmentRepository.GetByIdAsync(idDonVi);
            department.Tinh_Trang = Constants.States.Disabed.GetHashCode();
            department.Id_NV_CapNhat = userId;
            department.Ngay_CapNhat = DateTime.Now;
            await this.unitOfWork.DepartmentRepository.UpdateAsync(department);
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Save a Department
        /// </summary>
        /// <param name="departmentVM">The Department</param>
        /// <param name="userId">Who is doing this action</param>
        /// <returns>A task</returns>
        public async Task SaveAsync(DepartmentVM departmentVM, int userId)
        {
            if (departmentVM.Id_DonVi != 0)
            {
                // Edit
                var old = await this.unitOfWork.DepartmentRepository.GetByIdAsync(departmentVM.Id_DonVi);
                if (old == null)
                {
                    throw new Exception("Đơn vị này không tồn tại");
                }

                departmentVM.Ma_DonVi = old.Ma_DonVi;
                var entity = mapper.Map<Department>(departmentVM);
                entity.Ngay_CapNhat = DateTime.Now;
                entity.Id_NV_CapNhat = userId;
                entity.Ngay_KhoiTao = old.Ngay_KhoiTao;
                entity.Id_NV_KhoiTao = old.Id_NV_KhoiTao;
                entity.Tinh_Trang = old.Tinh_Trang;
                entity.Trang_Thai = old.Trang_Thai;

                entity.SMTP_Email = old.SMTP_Email;
                entity.Port_Email = old.Port_Email;
                entity.Pass_Email = old.Pass_Email;
                entity.Email = old.Email;
                entity.Account_Email = old.Account_Email;

                await this.unitOfWork.DepartmentRepository.UpdateAsync(entity);
            }
            else
            {
                var entity = mapper.Map<Department>(departmentVM);
                entity.Ngay_KhoiTao = DateTime.Now;
                entity.Tinh_Trang = Constants.States.Actived.GetHashCode();
                entity.Id_NV_KhoiTao = userId;
                await this.unitOfWork.DepartmentRepository.AddAsync(entity);
            }

            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Get department by id
        /// </summary
        /// <param name="id">The id of department</param>
        /// <returns>The department</returns>
        public async Task<Department> GetByIdAsync(int id)
        {
            return await this.unitOfWork.DepartmentRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// update email
        /// </summary>
        /// <param name="emailDepartment">the email of department vm</param>
        /// <param name="userId">the employee id current</param>
        /// <returns>the task</returns>
        public async Task UpdateEmailAsync(EmailDepartmentVM emailDepartment, int userId)
        {
            var department = await this.unitOfWork.DepartmentRepository.GetByIdAsync(emailDepartment.Id_DonVi);
            if (department != null)
            {
                department.Email = emailDepartment.Email;
                department.SMTP_Email = emailDepartment.SMTP_Email;
                department.Port_Email = emailDepartment.Port_Email;
                department.Account_Email = emailDepartment.Account_Email;
                department.Pass_Email = emailDepartment.Pass_Email;
                department.Id_NV_CapNhat = userId;
                department.Ngay_CapNhat = DateTime.Now;
                await this.unitOfWork.DepartmentRepository.UpdateAsync(department);

                this.unitOfWork.Commit();
            }
        }

        /// <summary>
        /// convert jstree
        /// </summary>
        /// <param name="list">the list departments</param>
        /// <returns>list departments converted</returns>
        private IEnumerable<Department> ConvertToTree(IEnumerable<Department> list, int idRoot = 0)
        {
            if (list != null && list.Any())
            {
                if (list.Count() == 1)
                {
                    return list;
                }
                // groups features by Id feature parent
                var groupDepartments = list.GroupBy(x => x.Id_DV_Cha);
                Department root = new() { Id_DV_Cha = 0 };

                foreach (var gr in groupDepartments)
                {
                    Department fr = list.FirstOrDefault(y => y.Id_DonVi == gr.Key);
                    if (fr != null)
                    {
                        if (fr.Id_DonVi == idRoot)
                        {
                            root = fr;
                        }

                        fr.Children = gr;
                    }
                };
                // Gets root features that has ID_CN_PR = 0

                return new List<Department> { root };
            }
            return null;
        }

        /// <summary>
        /// Gets children that are controlled by the account by parent Id
        /// </summary>
        /// <param name="parentId">parent department id</param>
        /// <param name="accountId">Account id</param>
        /// <returns>List department</returns>
        public async Task<IEnumerable<Department>> GetDepartmentsByIdParent(int idParent, int accountId)
        {
            return await this.unitOfWork.DepartmentRepository.GetDepartmentsByIdParent(idParent, accountId);
        }
    }
}
