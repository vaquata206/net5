using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="username">The usename</param>
        /// <param name="password">The password</param>
        /// <returns>Access token</returns>
        public async Task<AccountInfo> LoginAsync(string username, string password)
        {
            password = Common.ComputeSha256Hash(password);

            var sql = @"SELECT * FROM TAIKHOAN WHERE TenTaiKhoan = @username AND MatKhau = @password AND DAXOA=0";
            var account = await this.dbContext.QueryFirstOrDefaultAsync<Account>(sql: sql, param: new
            {
                username,
                password
            });

            if (account == null)
                return null;

            if (account.IsKhachHang)
            {
                sql = @"SELECT TK.Id, TK.TenTaiKhoan, KH.HoTen, TK.IsKhachHang, TK.IdKhachHang FROM KhachHang kh
                            JOIN TaiKhoan TK ON TK.IdKhachHang = KH.Id AND TK.IsKhachHang = 1
                            WHERE TK.Id = @id AND TK.DaXoa = 0 AND kh.daxoa = 0";
            }
            else
            {
                sql = @"SELECT TK.Id, TK.TenTaiKhoan, nv.HoTen, TK.IsKhachHang, TK.IdNhanVien, nv.IdVaiTro FROM NhanVien nv
                            JOIN TaiKhoan TK ON TK.IdNhanVien = nv.Id AND TK.IsKhachHang = 0
                            WHERE TK.Id = @id AND TK.DaXoa = 0 AND nv.daxoa = 0 ";
            }

            return await this.dbContext.QueryFirstOrDefaultAsync<AccountInfo>(
                sql: sql,
                param: new
                {
                    account.Id
                }
                );
        }

        /// <summary>
        /// Lay danh sach tai khoan theo id nhan vien
        /// </summary>
        /// <param name="id">id nhan vien</param>
        /// <returns>Danh sach tai khoan</returns>
        public async Task<IEnumerable<Account>> LayDsTaiKhoanTheoIdNhanVien(int id)
        {
            var sql = @"SELECT * FROM TaiKhoan
                        WHERE DaXoa = @daXoa AND IdNhanVien = @id";

            return await this.dbContext.QueryAsync<Account>(
                sql: sql,
                param: new
                {
                    id = id,
                    daXoa = Constants.TrangThai.ChuaXoa
                },
                commandType: CommandType.Text
            );
        }

        /// <summary>
        /// get account by username
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns>account</returns>
        public async Task<Account> GetAccountByUsername(string userName)
        {
            var sql = @"SELECT * FROM TaiKhoan
                        WHERE DaXoa = 0 AND TenTaiKhoan = @username";
            return await this.dbContext.QueryFirstOrDefaultAsync<Account>(
                sql: sql,
                param: new
                {
                    username = userName.ToLower()
                },
                commandType: CommandType.Text
                );
        }

        /// <summary>
        /// get account by email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>account</returns>
        public async Task<AccountInfo> GetAccountByEmail(string email)
        {
            var sql = string.Format(@"SELECT TK.Id, TK.TenTaiKhoan, TK.LoaiTaiKhoan,
                        TK.IdNhaDauTu, TK.IdNhanVien, TK.TrangThai,
                        DV.Ten AS TenDonVi, NV.IdDonVi,
                        (Case When TK.LoaiTaiKhoan = {0} Then NDT.TenNhaDauTu
                               When TK.LoaiTaiKhoan = {1} Then NV.HoTen END) AS HoTen
                        FROM TaiKhoan TK
                        LEFT JOIN NhanVien NV ON TK.IdNhanVien = NV.Id AND NV.DaXoa = 0
                        LEFT JOIN NhaDauTu NDT ON NDT.Id = TK.IdNhaDauTu AND NDT.DaXoa = 0
                        LEFT JOIN DONVI DV ON DV.Id = NV.IdDonVi
                        WHERE TK.DaXoa = 0  AND ( NV.Email = @email OR NDT.Email= @email) ",
                        Constants.LoaiTaiKhoan.NhaDauTu.GetHashCode(), Constants.LoaiTaiKhoan.NhanVien.GetHashCode());

            return await this.dbContext.QueryFirstOrDefaultAsync<AccountInfo>(
                sql: sql,
                param: new
                {
                    email = email.ToLower()
                },
                commandType: CommandType.Text
                );
        }

        /// <summary>
        /// get account by idNhaDauTu
        /// </summary>
        /// <param name="idNhaDauTu">idNhaDauTu</param>
        /// <returns>account</returns>
        public async Task<Account> GetAccountByIdNhaDauTu(int idNhaDauTu)
        {
            var sql = @"SELECT * FROM TaiKhoan
                        WHERE DaXoa = 0 AND IdNhaDauTu = @idNhaDauTu";
            return await this.dbContext.QueryFirstOrDefaultAsync<Account>(
                sql: sql,
                param: new
                {
                    idNhaDauTu = idNhaDauTu
                },
                commandType: CommandType.Text
                );
        }
    }
}
