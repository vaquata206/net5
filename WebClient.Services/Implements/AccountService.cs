using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;
using WebClient.Repositories;
using WebClient.Services.Interfaces;
using System.Linq;

namespace WebClient.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork unitOfWork;
        public AccountService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<AccountInfo> LoginAsync(LoginVM viewModel)
        {
            return await this.unitOfWork.AccountRepository.LoginAsync(viewModel.Username, viewModel.Password);
        }

        /// <summary>
        /// Change user's password
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="changePasswordVM">change passworld vm</param>
        /// <returns></returns>
        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordVM changePasswordVM)
        {
            var account = await this.unitOfWork.AccountRepository.GetByIdAsync(userId);
            return await this.unitOfWork.AccountRepository.ChangePasswordAsync(
                account.UserName, 
                account.Id_NguoiDung, 
                changePasswordVM.MatKhauCu, 
                changePasswordVM.MatKhauMoi);
        }

        /// <summary>
        /// Gets employee's accounts
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <returns>List account</returns>
        public async Task<IEnumerable<Account>> GetAccountsByEmployeeId(int id)
        {
            return await this.unitOfWork.AccountRepository.GetAccountsByEmployeeId(id);
        }

        public async Task<Account> GetByUsername(string userName)
        {
            return await this.unitOfWork.AccountRepository.GetAccountByUsername(userName);
        }

        /// <summary>
        /// Create a new account for a employee
        /// </summary>
        /// <param name="accountVM">Account VM</param>
        /// <param name="userId">Current user id</param>
        /// <returns>A void task</returns>
        public async Task CreateAccountAsync(AccountVM accountVM, int userId)
        {
            var account = new Account
            {
                Id_NhanVien = accountVM.Id_NhanVien,
                UserName = accountVM.UserName,
                MatKhau = Common.ComputeSha256Hash(accountVM.MatKhau),
                Id_NV_KhoiTao = userId,
                Ngay_KhoiTao = DateTime.Now,
                Ma_NguoiDung = "user" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                Tinh_Trang = Constants.States.Actived.GetHashCode(),
                Quan_Tri = 0,
                Id_VaiTro = 3,
            };

            await this.unitOfWork.AccountRepository.AddAsync(account);
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Delete a account
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="userId">Current user id</param>
        /// <returns>A account</returns>
        public async Task<Account> DeleteAccountAsync(int accountId, int userId)
        {
            var account = await this.unitOfWork.AccountRepository.GetByIdAsync(accountId, true);
            if (account.Quan_Tri == Constants.States.Actived.GetHashCode())
            {
                throw new Exception("Tài khoản này không được xóa");
            }

            account.Tinh_Trang = Constants.States.Disabed.GetHashCode();
            account.Ngay_CapNhat = DateTime.Now;
            account.Id_NV_CapNhat = userId;

            await this.unitOfWork.AccountRepository.UpdateAsync(account);
            this.unitOfWork.Commit();

            return account;
        }

        /// <summary>
        /// Reset password of a account
        /// </summary>
        /// <param name="accountId">Acount id</param>
        /// <param name="userId">currebt user</param>
        /// <returns></returns>
        public async Task<string> ResetPassword(int accountId, int userId)
        {
            var now = DateTime.Now;
            var currAccount = await this.unitOfWork.AccountRepository.GetByIdAsync(accountId);
            var password = CreatePassword(6);
            currAccount.MatKhau = Common.ComputeSha256Hash(password);
            currAccount.Ngay_DoiMatKhau = now;
            currAccount.SoLan_LoginSai = 0;
            currAccount.Id_NV_CapNhat = userId;
            currAccount.Ngay_CapNhat = now;

            await this.unitOfWork.AccountRepository.UpdateAsync(currAccount);
            this.unitOfWork.Commit();
            return password;
        }

        /// <summary>
        /// Generate random a password
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new();
            Random rnd = new();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        /// <summary>
        /// Gets accounts with tree node format
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>Tree nodes</returns>
        public async Task<IEnumerable<TreeNode>> GetTreeNodeAccounts(int employeeId)
        {
            var accounts = await this.unitOfWork.AccountRepository.GetAccountsByEmployeeId(employeeId);
            if (accounts == null)
            {
                return null;
            }

            return accounts.Select(x => new TreeNode
            {
                Children = false,
                Id = "A" + x.Id_NguoiDung,
                Text = x.UserName,
                TypeNode = "Account"
            });
        }
    }
}
