using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClient.Attributes;
using WebClient.Contexts;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Areas.Api.Controllers
{
    /// <summary>
    /// Api controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        public readonly IAccountService accountService;
        public readonly IBaoHongService baoHongService;

        public AccountController(
            IAccountService accountService,
            IBaoHongService baoHongService,
            ILogger<AccountController> logger,
            IContextFactory contextFactory) : base(contextFactory, logger)
        {
            this.baoHongService = baoHongService;
            this.accountService = accountService;
        }

        /// <summary>
        /// Login action
        /// </summary>
        /// <param name="login">Login vm</param>
        /// <returns>JWT token and info of account</returns>
        /// <response code="200">Returns info of account</response>
        /// <response code="400">If login failed</response>
        [HttpPost("[action]")]
        [AllowAnonymous]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginVM login)
        {
            try
            {
                var account = await this.accountService.LoginAsync(login);
                if (account == null)
                {
                    return this.BadRequest();
                }

                var loginResponse = this.BaseContext.GetLoginResponse(account);
                return this.Ok(loginResponse);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                return this.BadRequest();
            }
        }

        /// <summary>
        /// Check token
        /// </summary>
        /// <returns>return action result</returns>
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Check()
        {
            return this.Ok();
        }

        /// <summary>
        /// Check token
        /// </summary>
        /// <returns>return action result</returns>
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> BaoHong()
        {
            var list = await this.baoHongService.SearchAsync(new BaoHongSearch { 
                IdTrangThaiPhieu = 4,
                IdKhachHang = this.BaseContext.Account.IdKhachHang.Value
            });
            return this.Ok(list);
        }
    }
}
