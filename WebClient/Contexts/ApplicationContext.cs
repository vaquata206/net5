using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using WebClient.Core;
using WebClient.Core.Models;
using WebClient.Extensions;
using WebClient.Services.Interfaces;

namespace WebClient.Contexts
{
    public class ApplicationContext
    {
        /// <summary>
        /// Http context accessor
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Feature service
        /// </summary>
        private readonly IFeatureService featureService;

        private readonly AppSetting appSetting;

        /// <summary>
        /// Account info
        /// </summary>
        private AccountInfo account;

        /// <summary>
        /// List menu
        /// </summary>
        private IEnumerable<Menu> menu;

        /// <summary>
        /// A contrustor
        /// </summary>
        /// <param name="appSetting">App setting</param>
        /// <param name="httpContextAccessor">Http context accessor</param>
        /// <param name="featureService">Feature service</param>
        /// <param name="departmentService">Department service</param>
        public ApplicationContext(AppSetting appSetting, IHttpContextAccessor httpContextAccessor, IFeatureService featureService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.featureService = featureService;
            this.appSetting = appSetting;
        }

        /// <summary>
        /// Account of a user who is requested to the server
        /// </summary>
        public AccountInfo Account
        {
            get
            {
                if (this.account == null)
                {
                    var claims = this.httpContextAccessor.HttpContext.User.Claims;
                    if (claims == null || !claims.Any())
                    {
                        return null;
                    }

                    this.account = new AccountInfo
                    {
                        TenTaiKhoan = GetStringValueClaim(claims, ClaimAccountType.UserName),
                        Id = GetIntValueClaim(claims, ClaimAccountType.UserId),
                        HoTen = GetStringValueClaim(claims, ClaimAccountType.FullName),
                        IdNhanVien = GetNullableIntValueClaim(claims, ClaimAccountType.EmployeeId),
                        IdKhachHang = GetNullableIntValueClaim(claims, ClaimAccountType.CustomerId),
                        IsKhachHang = GetIntValueClaim(claims, ClaimAccountType.AccountType) == 0? false: true,
                        IdVaiTro = GetNullableIntValueClaim(claims, ClaimAccountType.RoleType)
                    };
                }

                return this.account;
            }
        }
        
        public IEnumerable<Menu> Menu
        {
            get
            {
                if (this.menu == null)
                {
                    var session = this.httpContextAccessor.HttpContext.Session;
                    IEnumerable<Menu> menu = session.GetObject<IEnumerable<Menu>>(SessionExtension.SessionKeyMenu);
                    if (menu == null || !menu.Any())
                    {
                        var account = this.Account;
                        // Get features that the user can access
                        menu = this.featureService.GetMenuAsync(account);

                        // Store modules to session
                        this.httpContextAccessor.HttpContext.Session.SetObject(SessionExtension.SessionKeyMenu, menu);
                    }

                    this.menu = menu;
                }

                return this.menu ?? Enumerable.Empty<Menu>();
            }
        }

        /// <summary>
        /// Current request
        /// </summary>
        public HttpRequest Request
        {
            get
            {
                return this.httpContextAccessor.HttpContext.Request;
            }
        }

        /// <summary>
        /// Login with the account
        /// </summary>
        /// <param name="account">The account</param>
        /// <returns>A void task</returns>
        public async Task LoginAsync(AccountInfo account)
        {
            var authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            // create identity
            ClaimsIdentity identity = CreateClaimsIdentity(account, authenticationScheme);

            // create principal
            ClaimsPrincipal principal = new(identity);

            await this.httpContextAccessor.HttpContext.SignInAsync(
                    scheme: authenticationScheme,
                    principal: principal,
                    properties: new AuthenticationProperties
                    {
                        // IsPersistent = true, // for 'remember me' feature
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(appSetting.ExpiredTicket)
                    });
        }

        /// <summary>
        /// Logout current account
        /// </summary>
        /// <returns>A void task</returns>
        public async Task LogoutAsync()
        {
            // Logout current user
            await this.httpContextAccessor.HttpContext.SignOutAsync();

            // Remove all entities from current session
            this.httpContextAccessor.HttpContext.Session.Clear();
        }

        /// <summary>
        /// Creates claims indentity from account info
        /// </summary>
        /// <param name="account">Account info</param>
        /// <param name="authenticationType">Authentication type</param>
        /// <returns>Claims Identity</returns>
        private static ClaimsIdentity CreateClaimsIdentity(AccountInfo account, string authenticationType = null)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimAccountType.UserName, account.TenTaiKhoan),
                new Claim(ClaimAccountType.UserId, account.Id.ToString()),
                new Claim(ClaimAccountType.FullName, account.HoTen.ToString()),
                new Claim(ClaimAccountType.EmployeeId, account.IdNhanVien > 0 ? account.IdNhanVien.ToString() : string.Empty),
                new Claim(ClaimAccountType.CustomerId, account.IdKhachHang > 0 ? account.IdKhachHang.ToString() : string.Empty),
                new Claim(ClaimAccountType.AccountType, account.IsKhachHang ? "1" : "0"),
                new Claim(ClaimAccountType.RoleType, account.IdVaiTro > 0 ? account.IdVaiTro.ToString() : string.Empty)
            };

            ClaimsIdentity claimsIdentity;
            if (string.IsNullOrEmpty(authenticationType))
            {
                claimsIdentity = new ClaimsIdentity(claims);
            }
            else
            {
                claimsIdentity = new ClaimsIdentity(claims, authenticationType);
            }

            return claimsIdentity;
        }

        /// <summary>
        /// Get value claim with string type
        /// </summary>
        /// <param name="claims">List claim</param>
        /// <param name="type">type claim</param>
        /// <returns>value of the claim</returns>
        private static string GetStringValueClaim(IEnumerable<Claim> claims, string type)
        {
            var value = string.Empty;
            if (claims != null)
            {
                value = claims.Where(x => x.Type == type)
                    .Select(x => x.Value).SingleOrDefault();
            }

            return value;
        }

        /// <summary>
        /// Get value claim with string type
        /// </summary>
        /// <param name="claims">List claim</param>
        /// <param name="type">type claim</param>
        /// <returns>value of the claim</returns>
        private static int GetIntValueClaim(IEnumerable<Claim> claims, string type)
        {
            var valueString = GetStringValueClaim(claims, type);
            _ = int.TryParse(valueString, out int valueInt);
            return valueInt;
        }

        private static int? GetNullableIntValueClaim(IEnumerable<Claim> claims, string type)
        {
            var valueString = GetStringValueClaim(claims, type);
            if (string.IsNullOrEmpty(valueString))
            {
                return null;
            }

            _ = int.TryParse(valueString, out int valueInt);
            return valueInt;
        }
    }
}
