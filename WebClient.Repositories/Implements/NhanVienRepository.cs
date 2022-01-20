using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class NhanVienRepository : BaseRepository<NhanVien>, INhanVienRepository
    {
        public NhanVienRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Lay danh sach nhan vien theo id don vi
        /// </summary>
        /// <param name="idDonVi">id don vi</param>
        /// <returns>danh sach nhan vien</returns>
        public async Task<IEnumerable<NhanVien>> LayDsNhanVienTheoIdDonVi(int idDonVi)
        {
            var sql = @"Select * FROM NhanVien Where IdDonVi = @idDonVi and DaXoa = @daXoa";
            return await this.dbContext.QueryAsync<NhanVien>(
                sql: sql,
                param: new
                {
                    idDonVi = idDonVi,
                    daXoa = Constants.TrangThai.ChuaXoa
                },
                commandType: CommandType.Text
            );
        }

        /// <summary>
        /// Get employees by chức vụ
        /// </summary>
        /// <param name="deparmentId">Chức vụ</param>
        /// <returns>List employee</returns>
        public async Task<IEnumerable<NhanVien>> GetEmployeesByChucVuAsync(int chucVu)
        {
            var query = @"Select * FROM Nhan_Vien Where Chuc_Vu = :chucVu and Tinh_Trang = 1";
            return await this.dbContext.QueryAsync<NhanVien>(
                sql: query,
                param: new { chucVu });
        }

        public async Task<IEnumerable<NhanVien>> GetNhanVienKyThuat()
        {
            var sql = "select * from NhanVien where daxoa = 0 and idvaitro = 1";
            return await this.dbContext.QueryAsync<NhanVien>(
                sql: sql);
        }

    }
}
