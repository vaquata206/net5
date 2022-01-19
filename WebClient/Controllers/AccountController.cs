using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClient.Attributes;
using WebClient.Contexts;
using WebClient.Core.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService accountService;
        private readonly IEmployeeService employeeService;
        /// <summary>
        /// Account controller
        /// </summary>
        /// <param name="accountService">account service</param>
        /// <param name="logger">Logger</param>
        /// <param name="contextFactory">Content factory</param>
        public AccountController(
            IAccountService accountService,
            IEmployeeService employeeService,
            ILogger<AccountController> logger,
            IContextFactory contextFactory) : base (contextFactory, logger)
        {
            this.accountService = accountService;
            this.employeeService = employeeService;
        }

        /// <summary>
        /// Action login
        /// </summary>
        /// <returns>The login page</returns>
        [AllowAnonymous]
        [Route("/login")]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Form submit login
        /// </summary>
        /// <param name="viewModel">Login info</param>
        /// <returns>Redirect to home page or ReturnUrl</returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateModelStateAttribute(RedirectTo = "index")]
        public async Task<IActionResult> Login(LoginVM viewModel)
        {
            try
            {
                var account = await this.accountService.LoginAsync(viewModel);
                if (account != null && !string.IsNullOrEmpty(account.UserCode))
                {
                    await this.BaseContext.LoginAsync(account);
                    var returnUrl = this.Request.Query["ReturnUrl"];
                    returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;

                    this.SetSuccessAlert("Đăng nhập thành công");
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception)
            {
            }
            
            this.TempData["MessageError"] = "Tên đăng nhập hoặc mật khẩu không đúng";
            return this.Redirect("/login");
        }

        /// <summary>
        /// Profile's account
        /// </summary>
        /// <returns>Profile page</returns>
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var account = this.BaseContext.Account;
            var employee = await this.employeeService.GetByIdAsync(account.EmployeeId);
            return this.View(employee);
        }

        /// <summary>
        /// Update employee
        /// </summary>
        /// <param name="employeeVM">the newEmployee</param>
        /// <returns>view profile</returns>
        [HttpPost]
        [Authorize]
        [ValidateModelState(RedirectTo = "/", Redirect = ValidateModelStateAttribute.RedirectType.URL)]
        public async Task<IActionResult> Profile(EmployeeVM employeeVM)
        {
            try
            {
                await this.employeeService.UpdateProfileAsync(employeeVM, this.BaseContext.Account.UserId);
                this.SetSuccessAlert("Cập nhập thông tin nhân viên thành công");
            }
            catch (Exception)
            {
                this.SetErrorAlert("Có lỗi xảy ra, xin vui lòng thử lại sau");
            }

            return this.RedirectToAction("Profile");
        }

        /// <summary>
        /// Change password controller
        /// </summary>
        /// <param name="changePasswordVM">the VM</param>
        /// <returns>the view</returns>
        [HttpPost]
        [Authorize]
        [ValidateModelState(RedirectTo = "profile")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (!changePasswordVM.MatKhauMoi.Equals(changePasswordVM.XacNhanMatKhau))
            {
                this.SetErrorAlert("Xác nhận mật khẩu không chính xác.");
            }
            else
            {
                var account = this.BaseContext.Account;
                var isSuccess = await this.accountService.ChangePasswordAsync(account.UserId, changePasswordVM);
                if (isSuccess)
                {
                    this.SetSuccessAlert("Đổi mật khẩu thành công");
                }
                else
                {
                    this.SetErrorAlert("Đổi mật khẩu không thành công. Mật khẩu cũ không chính xác.");
                }
            }

            return this.RedirectToAction("Profile");
        }

        /// <summary>
        /// Create a acount for the employee
        /// </summary>
        /// <param name="accountVM">Account VM</param>
        /// <returns>Redirects to the detail page</returns>
        [ValidateAntiForgeryToken]
        [ValidateModelState(RedirectTo = "/employee", Redirect = ValidateModelStateAttribute.RedirectType.URL)]
        [Permission(Controller = "employee", Action = "index")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountVM accountVM)
        {
            var existAccount = await this.accountService.GetByUsername(accountVM.UserName);
            if (existAccount != null)
            {
                this.SetErrorAlert(string.Format("Tên đăng nhập {0} đã tồn tại", accountVM.UserName));
            }
            else
            {
                await this.accountService.CreateAccountAsync(accountVM, this.BaseContext.Account.UserId);
                this.SetSuccessAlert(string.Format("Tạo tài khoản {0} thành công", accountVM.UserName));
            }

            this.TempData["TabActive"] = "account";
            return this.Redirect("/employee/detail/" + accountVM.Id_NhanVien);
        }

        /// <summary>
        /// Delete a account
        /// </summary>
        /// <param name="idNguoiDung">account id</param>
        /// <returns>A Action result</returns>        
        [HttpPost]
        [Permission(Controller = "employee", Action = "index")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            try
            {
                var account = await this.accountService.DeleteAccountAsync(accountId, this.BaseContext.Account.UserId);
                this.SetSuccessAlert(string.Format("Xóa tài khoản {0} thành công", account.UserName));
                this.TempData["TabActive"] = "account";
                return this.Redirect("/employee/detail/" + account.Id_NhanVien);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert();
                return this.Redirect("/employee");
            }
        }

        /// <summary>
        /// Reset password of account
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <returns>A new password</returns>
        [Permission(Controller = "employee", Action = "index")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(int accountId)
        {
            var password = await this.accountService.ResetPassword(accountId, this.BaseContext.Account.UserId);
            return this.Ok(password);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            await this.BaseContext.LogoutAsync();
            return this.Redirect("/");
        }
    }
}
