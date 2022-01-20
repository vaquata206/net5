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
    public class DonViService: IDonViService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DonViService(IMapper mapper, IUnitOfWork unitOfWork)
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

            var user = await this.unitOfWork.AccountRepository.GetByIdAsync(accountId);
            var departments = await this.unitOfWork.DepartmentRepository.GetDepartmentsByIdParent(departmentId, accountId);

            result = departments.Select(x => new TreeNode
            {
                Id = "D" + x.Id,
                Children = true,
                Text = x.Ten,
                TypeNode = "Deparment"
            });

            var employees = await this.unitOfWork.NhanVienRepository.LayDsNhanVienTheoIdDonVi(departmentId);

            result = result.Concat(employees.Select(x => new TreeNode
            {
                Id = "E" + x.Id,
                Children = true,
                Text = x.HoTen,
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
        public async Task<IEnumerable<DonVi>> GetTreeDepartmentsdWithTerm(int userId, int departmentId, int idDepartmentEnd = -1)
        {
            var departments = await this.unitOfWork.DepartmentRepository.GetDepartmentsdWithTerm(userId, departmentId, idDepartmentEnd);
            if (departmentId != idDepartmentEnd)
            {
                if (idDepartmentEnd != -1)
                {
                    var departmentsRs = departments.Where(x => x.Id != idDepartmentEnd).ToList();
                    return this.ConvertToTree(departmentsRs, departmentId);
                }
            }
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
            department.DaXoa = Constants.TrangThai.DaXoa;
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
            if (departmentVM.Id != 0)
            {
                // Edit
                var old = await this.unitOfWork.DepartmentRepository.GetByIdAsync(departmentVM.Id);
                if (old == null)
                {
                    throw new Exception("Đơn vị này không tồn tại");
                }

                departmentVM.Ma = old.Ma;
                var entity = mapper.Map<DonVi>(departmentVM);
                entity.DaXoa = old.DaXoa;

                await this.unitOfWork.DepartmentRepository.UpdateAsync(entity);
            }
            else
            {
                var entity = mapper.Map<DonVi>(departmentVM);
                entity.DaXoa = Constants.TrangThai.ChuaXoa;
                await this.unitOfWork.DepartmentRepository.AddAsync(entity);
            }

            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Get department by id
        /// </summary
        /// <param name="id">The id of department</param>
        /// <returns>The department</returns>
        public async Task<DonVi> GetByIdAsync(int id)
        {
            return await this.unitOfWork.DepartmentRepository.GetByIdAsync(id);
        }

        private IEnumerable<DonVi> ConvertToTree(IEnumerable<DonVi> list, int idRoot = 0)
        {
            if (list != null && list.Any())
            {
                if (list.Count() == 1)
                {
                    return list;
                }
                // groups features by Id feature parent
                var groupDepartments = list.GroupBy(x => x.DonViCha);
                DonVi root = new() { DonViCha = 0 };

                foreach (var gr in groupDepartments)
                {
                    DonVi fr = list.FirstOrDefault(y => y.Id == gr.Key);
                    if (fr != null)
                    {
                        if (fr.Id == idRoot)
                        {
                            root = fr;
                        }

                        fr.Children = gr;
                    }
                };
                // Gets root features that has ID_CN_PR = 0

                return new List<DonVi> { root };
            }
            return null;
        }

        /// <summary>
        /// Gets children that are controlled by the account by parent Id
        /// </summary>
        /// <param name="parentId">parent department id</param>
        /// <param name="accountId">Account id</param>
        /// <returns>List department</returns>
        public async Task<IEnumerable<DonVi>> GetDepartmentsByIdParent(int idParent, int accountId)
        {
            return await this.unitOfWork.DepartmentRepository.GetDepartmentsByIdParent(idParent, accountId);
        }
    }
}
