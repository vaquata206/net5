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
    public class NhanVienService : INhanVienService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public NhanVienService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// get employee view model by employee id
        /// </summary>
        /// <param name="nhanVienVM">employee's id</param>
        /// <returns>the employee instance</returns>
        public async Task<NhanVien> GetByIdAsync(int employeeId)
        {
            return await this.unitOfWork.NhanVienRepository.GetByIdAsync(employeeId);
        }

        /// <summary>
        /// update profile
        /// </summary>
        /// <param name="nhanVienVM">the employee VM</param>
        /// <param name="userId">user id</param>
        /// <returns>A task</returns>
        public async Task UpdateProfileAsync(NhanVienVM nhanVienVM, int userId)
        {
            NhanVien nhanVien = this.mapper.Map<NhanVien>(nhanVienVM);

            if (nhanVienVM.Id > 0)
            {
                var oldEmp = await this.unitOfWork.NhanVienRepository.GetByIdAsync(nhanVienVM.Id);
                nhanVien.DaXoa = Constants.TrangThai.ChuaXoa;

                await this.unitOfWork.NhanVienRepository.UpdateAsync(nhanVien);
                this.unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Lay danh sach nhan vien theo id don vi
        /// </summary>
        /// <param name="idDonVi">id don vi</param>
        /// <returns>danh sach nhan vien</returns>
        public async Task<IEnumerable<NhanVien>> LayDsNhanVienTheoIdDonVi(int deparmentId)
        {
            return await this.unitOfWork.NhanVienRepository.LayDsNhanVienTheoIdDonVi(deparmentId);
        }

        /// <summary>
        /// Get employees by chức vụ
        /// </summary>
        /// <param name="chucVu">Chức vụ</param>
        /// <returns>List employee</returns>
        public async Task<IEnumerable<NhanVien>> GetEmployeesByChucVuAsync(int chucVu)
        {
            return await this.unitOfWork.NhanVienRepository.GetEmployeesByChucVuAsync(chucVu);
        }

        /// <summary>
        /// Xoa thong tin nhan vien
        /// </summary>
        /// <param name="idNhanVien">id nahn vien</param>
        /// <param name="userId">Current user id</param>
        /// <returns>the Task</returns>
        public async Task DeleteByIdAsync(int idNhanVien, int userId)
        {
            var nv = await this.unitOfWork.NhanVienRepository.GetByIdAsync(idNhanVien);
            nv.DaXoa = Constants.TrangThai.DaXoa;
            await this.unitOfWork.NhanVienRepository.UpdateAsync(nv);
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Luu thong tin nhan vien (insert or update)
        /// </summary>
        /// <param name="nhanVienVM">nhan vien VM</param>
        /// <param name="userId">the current userid</param>
        /// <returns>the task</returns>
        public async Task SaveAsync(NhanVienVM nhanVienVM, int userId)
        {
            NhanVien nv = this.mapper.Map<NhanVien>(nhanVienVM);
            //var department = await this.unitOfWork.DepartmentRepository.GetByIdAsync(nv.IdDonVi);
            //if (department == null)
            //{
            //    throw new Exception("Đơn vị không đúng");
            //}

            nv.DaXoa = Constants.TrangThai.ChuaXoa;
            if (nhanVienVM.Id > 0)
            {
                var oldEmp = await this.unitOfWork.NhanVienRepository.GetByIdAsync(nhanVienVM.Id);

                await this.unitOfWork.NhanVienRepository.UpdateAsync(nv);
            }
            else
            {
                await this.unitOfWork.NhanVienRepository.AddAsync(nv);
            }

            this.unitOfWork.Commit();
        }
    }
}
