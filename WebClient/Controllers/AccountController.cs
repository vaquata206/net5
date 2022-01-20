using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClient.Attributes;
using WebClient.Contexts;
using WebClient.Core.Helpers;
using WebClient.Core.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService accountService;
        /// <summary>
        /// Account controller
        /// </summary>
        /// <param name="accountService">Account service</param>
        /// <param name="logger">logger</param>
        /// <param name="contextFactory">Context factory</param>
        public AccountController(
            IAccountService accountService,
            ILogger<HomeController> logger,
            IContextFactory contextFactory) : base(contextFactory, logger)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// Action login
        /// </summary>
        /// <returns>The login page</returns>
        [AllowAnonymous]
        [Route("/login")]
        [HttpGet]
        public IActionResult Login()
        {
            this.ViewBag.ReturnUrl = this.Request.Query["ReturnUrl"];
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
                if (account != null)
                {
                    await this.BaseContext.LoginAsync(account);
                    var returnUrl = this.Request.Query["ReturnUrl"];
                    var defaultUrl = account.IsKhachHang ? "/baohong" : "/";
                    returnUrl = string.IsNullOrEmpty(returnUrl.ToString().Trim('/')) ? defaultUrl : returnUrl;

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
        /// Logout
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("/logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Logout
                await this.BaseContext.LogoutAsync();
                return this.Redirect("/");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
            }

            return this.Redirect("/");
        }
    }
}
