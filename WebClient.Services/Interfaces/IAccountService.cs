using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    /// <summary>
    /// IAccount service
    /// </summary>
    public interface IAccountService
    {
        Task<AccountInfo> LoginAsync(LoginVM viewModel);
        
        /// <summary>
        /// Change user's password
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="changePasswordVM">change passworld vm</param>
        /// <returns></returns>
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordVM changePasswordVM);

        /// <summary>
        /// Gets employee's accounts
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <returns>List account</returns>
        Task<IEnumerable<Account>> GetAccountsByEmployeeId(int id);
        Task<Account> GetByUsername(string userName);

        /// <summary>
        /// Create a new account for a employee
        /// </summary>
        /// <param name="accountVM">Account VM</param>
        /// <param name="userId">Current user id</param>
        /// <returns>A void task</returns>
        Task CreateAccountAsync(AccountVM accountVM, int userId);

        /// <summary>
        /// Delete a account
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="userId">Current user id</param>
        /// <returns>A account</returns>
        Task<Account> DeleteAccountAsync(int accountId, int userId);

        /// <summary>
        /// Reset password of a account
        /// </summary>
        /// <param name="accountId">Acount id</param>
        /// <param name="userId">currebt user</param>
        /// <returns></returns>
        Task<string> ResetPassword(int accountId, int userId);


        /// <summary>
        /// Gets accounts with tree node format
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>Tree nodes</returns>
        Task<IEnumerable<TreeNode>> GetTreeNodeAccounts(int employeeId);
    }
}
