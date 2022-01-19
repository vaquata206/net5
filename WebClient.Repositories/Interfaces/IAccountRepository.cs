using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;

namespace WebClient.Repositories.Interfaces
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<AccountInfo> LoginAsync(string username, string password);
        Task<bool> ChangePasswordAsync(string userName, int id_NguoiDung, string matKhauCu, string matKhauMoi);

        /// <summary>
        /// Gets employee's accounts
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <returns>List account</returns>
        Task<IEnumerable<Account>> GetAccountsByEmployeeId(int id);

        /// <summary>
        /// get account by username
        /// </summary>
        /// <param name="userName">username</param>
        /// <returns>account</returns>
        Task<Account> GetAccountByUsername(string userName);
    }
}
