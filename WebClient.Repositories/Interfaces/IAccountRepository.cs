using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;

namespace WebClient.Repositories.Interfaces
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<AccountInfo> LoginAsync(string username, string password);

        /// <summary>
        /// Lay danh sach tai khoan theo id nhan vien
        /// </summary>
        /// <param name="id">id nhan vien</param>
        /// <returns>Danh sach tai khoan</returns>
        Task<IEnumerable<Account>> LayDsTaiKhoanTheoIdNhanVien(int id);

        /// <summary>
        /// get account by username
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns>account</returns>
        Task<Account> GetAccountByUsername(string userName);

        /// <summary>
        /// get account by email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>account</returns>
        Task<AccountInfo> GetAccountByEmail(string email);

        /// <summary>
        /// get account by idNhaDauTu
        /// </summary>
        /// <param name="idNhaDauTu">idNhaDauTu</param>
        /// <returns>account</returns>
        Task<Account> GetAccountByIdNhaDauTu(int idNhaDauTu);

        /// Tim kiem danh sach tai khoan nha dau tu
        /// </summary>
        /// <param name="request">filter TaiKhoanNhaDauTuFilter</param>
        /// <returns>Danh sach TaiKhoanNhaDauTuInfo</returns>
    }
}
