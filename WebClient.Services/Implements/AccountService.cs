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
            var response = await this.unitOfWork.AccountRepository.LoginAsync(viewModel.Username, viewModel.Password);
            return response;
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
            if (Common.ComputeSha256Hash(changePasswordVM.MatKhauCu) == account.MatKhau)
            {
                account.MatKhau = Common.ComputeSha256Hash(changePasswordVM.MatKhauMoi);

                await this.unitOfWork.AccountRepository.UpdateAsync(account);
                this.unitOfWork.Commit();
                return true;
            }
            else { 
                return false;
            }
        }

        /// <summary>
        /// Send email reser password
        /// </summary>
        /// <param name="email">email reset</param>
        /// <param name="sqlConnection">sqlConnection</param>
        /// <param name="dbTransaction">IDbTransaction</param>
        /// <returns></returns>
        public async Task SendEmailResetPassword(string email)
        {
            var user = await this.unitOfWork.AccountRepository.GetAccountByEmail(email);
            if (user == null)
            {
                throw new Exception("Email không tồn tại trong hệ thống!");
            }
            var title = Constants.ResetPassword.TitleEmail;
            string dauVaCuoiMail = System.IO.File.ReadAllText(@"./wwwroot/templates/DauVaCuoiMail.html");
            string noiDungGoiDoiTac = dauVaCuoiMail.Replace("{{NOIDUNG}}", 
                System.IO.File.ReadAllText(@"./wwwroot/templates/Mail_ResetPassword.html"));

            var hostMail = Constants.EmailHeThong.HostMail;
            var portMail = Constants.EmailHeThong.PortMail;
            var username = Constants.EmailHeThong.Username;
            var password = Constants.EmailHeThong.Password;

            var resetPassword = Common.GenerateResetPassword();
            DateTime now = DateTime.Now;
            var account = await this.unitOfWork.AccountRepository.GetByIdAsync(user.Id);
            var matKhauMoi = Common.ComputeSha256Hash(resetPassword);
            account.MatKhau = matKhauMoi;

            await this.unitOfWork.AccountRepository.UpdateAsync(account);
            var bodyMail = noiDungGoiDoiTac.Replace("{{HOTEN}}", user.HoTen)
                                    .Replace("{{TENTAIKHOAN}}", user.TenTaiKhoan)
                                    .Replace("{{MATKHAU}}", resetPassword);
            Common.SendMail(email, title, bodyMail, hostMail, portMail, username, password);
        }

        /// <summary>
        /// Lay danh sach tai khoan theo id nhan vien
        /// </summary>
        /// <param name="id">id nhan vien</param>
        /// <returns>Danh sach tai khoan</returns>
        public async Task<IEnumerable<Account>> LayDsTaiKhoanTheoIdNhanVien(int id)
        {
            return await this.unitOfWork.AccountRepository.LayDsTaiKhoanTheoIdNhanVien(id);
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
                TenTaiKhoan = accountVM.UserName.ToLower(),
                MatKhau = Common.ComputeSha256Hash(accountVM.MatKhau),
                IdNhanVien = accountVM.IdNhanVien,
                DaXoa = Constants.TrangThai.ChuaXoa,
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
            account.DaXoa = Constants.TrangThai.DaXoa;

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
            var accounts = await this.unitOfWork.AccountRepository.LayDsTaiKhoanTheoIdNhanVien(employeeId);
            if (accounts == null)
            {
                return null;
            }

            return accounts.Select(x => new TreeNode
            {
                Children = false,
                Id = "A" + x.Id,
                Text = x.TenTaiKhoan,
                TypeNode = "Account"
            });
        }
    }
}
