using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Update information's Employee
        /// </summary>
        /// <param name="employee">the employee</param>
        /// <param name="userId">user id</param>
        /// <returns>the employee after updated</returns>
        public async Task<Employee> UpdateProfileAsync(EmployeeVM employee, int userId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nv_capnhat", OracleDbType.Int64, ParameterDirection.Input, userId);
            dyParam.Add("p_id_nhanvien", OracleDbType.Varchar2, ParameterDirection.Input, employee.Id_NhanVien);
            dyParam.Add("p_ho_ten", OracleDbType.Varchar2, ParameterDirection.Input, employee.HoTen);
            dyParam.Add("p_dia_chi", OracleDbType.Varchar2, ParameterDirection.Input, employee.DiaChi);
            dyParam.Add("p_dien_thoai", OracleDbType.Varchar2, ParameterDirection.Input, employee.DienThoai);
            dyParam.Add("p_email", OracleDbType.Varchar2, ParameterDirection.Input, employee.Email);
            dyParam.Add("p_nam_sinh", OracleDbType.Date, ParameterDirection.Input, DateTime.ParseExact(employee.NamSinh, Constants.FormatDate, null));
            dyParam.Add("p_so_cmnd", OracleDbType.Varchar2, ParameterDirection.Input, employee.SoCMND);
            dyParam.Add("p_ngaycap_cmnd", OracleDbType.Date, ParameterDirection.Input, DateTime.ParseExact(employee.NgayCapCMND, Constants.FormatDate, null));
            dyParam.Add("p_noicap_cmnd", OracleDbType.Varchar2, ParameterDirection.Input, employee.NoiCapCMND);
            dyParam.Add("p_ghi_chu", OracleDbType.Varchar2, ParameterDirection.Input, employee.GhiChu);
            dyParam.Add("p_chuc_vu", OracleDbType.Int64, ParameterDirection.Input, employee.Chuc_Vu);
            dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.dbContext.QueryFirstOrDefaultAsync<Employee>(
                sql: "BLDT_EMPLOYEE.UPDATE_EMPLOYEE", 
                param: dyParam, 
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Get employees by department id
        /// </summary>
        /// <param name="deparmentId">Id of deparment</param>
        /// <returns>List employee</returns>
        public async Task<IEnumerable<Employee>> GetEmployeesByDeparmentIdAsync(int deparmentId)
        {
            var query = @"Select * FROM Nhan_Vien Where Id_DonVi = :deparmentId and Tinh_Trang = 1";
            return await this.dbContext.QueryAsync<Employee>(
                sql: query, 
                param: new { deparmentId });
        }

        /// <summary>
        /// Get employees by chức vụ
        /// </summary>
        /// <param name="deparmentId">Chức vụ</param>
        /// <returns>List employee</returns>
        public async Task<IEnumerable<Employee>> GetEmployeesByChucVuAsync(int chucVu)
        {
            var query = @"Select * FROM Nhan_Vien Where Chuc_Vu = :chucVu and Tinh_Trang = 1";
            return await this.dbContext.QueryAsync<Employee>(
                sql: query,
                param: new { chucVu });
        }
    }
}
