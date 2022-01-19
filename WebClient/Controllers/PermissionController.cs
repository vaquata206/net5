using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Attributes;
using WebClient.Contexts;
using WebClient.Core.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    /// <summary>
    /// Permission controller
    /// </summary>
    [Authorize]
    public class PermissionController : BaseController
    {
        /// <summary>
        /// Permission service
        /// </summary>
        private readonly IPermissionService permissionService;

        /// <summary>
        /// feature Service
        /// </summary>
        private readonly IFeatureService featureService;

        /// <summary>
        /// Department service
        /// </summary>
        private readonly IDepartmentService departmentService;

        /// <summary>
        /// Account service
        /// </summary>
        private readonly IAccountService accountService;

        /// <summary>
        /// Permission Feature Service
        /// </summary>
        private readonly IPermissionFeatureService permissionFeatureService;

        /// EmployeePermission Service
        /// </summary>
        private readonly IEmployeePermissionService employeePermissionService;

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="permissionService">Permission service instance</param>
        /// <param name="featureService">Feature service instance</param>
        /// <param name="departmentService">Department service</param>
        /// <param name="accountService">Account service</param>
        /// <param name="permissionFeatureService">permission feature service</param>
        /// <param name="contextFactory">Context factory</param>
        /// <param name="logger">The Logger</param>
        public PermissionController(
            IPermissionService permissionService, 
            IFeatureService featureService, 
            IDepartmentService departmentService,
            IAccountService accountService,
            IPermissionFeatureService permissionFeatureService,
            IEmployeePermissionService employeePermissionService,
            IContextFactory contextFactory,
            ILogger<PermissionController> logger) : base(contextFactory, logger)
        {
            this.permissionService = permissionService;
            this.featureService = featureService;
            this.departmentService = departmentService;
            this.employeePermissionService = employeePermissionService;
            this.accountService = accountService;
            this.permissionFeatureService = permissionFeatureService;
        }

        /// <summary>
        /// Action index
        /// </summary>
        /// <returns>Index page</returns>
        [Permission]
        public async Task<IActionResult> Index()
        {
            ViewBag.departmentIdCurrent = this.BaseContext.Account.DepartmentId;
            var permissions = await this.permissionService.GetPermissions();
            return this.View(permissions);
        }

        /// <summary>
        /// Action List of permission
        /// </summary>
        /// <returns>List Permission page</returns>
        [Permission]
        public async Task<IActionResult> List()
        {
            var list = await this.permissionService.GetPermissions();
            var features = await this.featureService.GetAllAsync();
            this.ViewBag.Features = features;
            return this.View(list);
        }

        /// <summary>
        /// Action control group permission
        /// </summary>
        /// <param name="id">Id of permistion</param>
        /// <returns>The page</returns>
        [HttpGet]
        [Permission(Action = "index")]
        public async Task<IActionResult> Detail(int id)
        {
            if (id == 0)
            {
                return this.View();
            }

            var permission = await this.permissionService.GetByIdAsync(id);

            if (permission == null)
            {
                return this.Redirect("error/404");
            }

            return this.View(permission);
        }


        /// <summary>
        /// Save a permission 
        /// </summary>
        /// <param name="permissionVM">Permission viewmodel</param>
        /// <returns>A Action result</returns>
        [ValidateAntiForgeryToken]
        [Permission(Action = "index")]
        [ValidateModelState(RedirectTo = "detail")]
        public async Task<IActionResult> Detail(PermissionVM permissionVM)
        {
            try
            {
                var permission = await this.permissionService.SaveAsync(permissionVM);
                this.SetSuccessAlert("Lưu thành công");
                return this.View(permission);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert(ex.Message);
            }

            return this.Redirect("/permission");
        }

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permissionId">Id of permission</param>
        /// <returns>Redirect to List page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Action = "index")]
        public async Task<IActionResult> Delete(int permissionId)
        {
            try
            {
                await this.permissionService.DeleteAsync(permissionId);
                this.SetSuccessAlert("Xóa thành công");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert();
            }

            return this.RedirectToAction("List");
        }

        /// <summary>
        /// Sets departments that the account is avaiabled work
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="departmentIds">Department ids</param>
        /// <returns>A action result</returns>
        [HttpPost]
        public async Task<IActionResult> SetDepartments(int accountId, int[] departmentIds)
        {
            try
            {
                await this.permissionService.SetDepartmentsAsync(
                    accountId, 
                    departmentIds,
                    this.BaseContext.Account.UserId);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Get features by permissionId
        /// </summary>
        /// <param name="permissionId">Id of permission</param>
        /// <returns>List id of feature</returns>
        [HttpGet]
        [Permission(Action = "index")]
        public async Task<IActionResult> GetFeaturesByPermissionId(int permissionId)
        {
            try
            {
                var list = await this.permissionFeatureService.GetPermissionFeaturesByPermissionId(permissionId);
                return this.Ok(list.Select(x => x.Id_ChucNang));
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                return this.BadRequest(ex.Message);
            } 
        }

        /// Get children of Deparment
        /// </summary>
        /// <param name="idDeparment">Id of deparment</param>
        /// <returns>list of deparment</returns>
        [HttpPost]
        public async Task<IActionResult> GetChildrenOfDeparment(int idDeparment)
        {
            try
            {
                var list = await this.departmentService.GetTreeNodeChildrenOfDepartment(idDeparment, this.BaseContext.Account.UserId);
                return this.Ok(list);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Set feature for permission
        /// </summary>
        /// <param name="features">id features</param>
        /// <param name="permissionId">Id of permission</param>
        /// <returns>A action result</returns>
        [HttpPost]
        [Permission(Action = "index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetFeatures(int[] features, int permissionId)
        {
            try
            {
                await this.permissionFeatureService.SetFeaturesForPermissionAsync(
                    features.AsEnumerable(),
                    permissionId,
                    this.BaseContext.Account.EmployeeId);

                this.SetSuccessAlert("Cấp chức năng cho quyền thành công.");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert("Đã xảy ra lỗi trong quá trình thực hiện.");
            }

            return this.RedirectToAction("List");
        }

        /// Get accounts of employee with tree node format
        /// </summary>
        /// <param name="idEmployee">Employee id</param>
        /// <returns>list account</returns>
        public async Task<IActionResult> GetAccountsOfEmployee(int idEmployee)
        {
            var list = await this.accountService.GetTreeNodeAccounts(idEmployee);
            return this.Ok(list);
        }

        /// <summary>
        /// Get Id permissions of a user;
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List id</returns>
        [HttpGet]
        public async Task<IActionResult> GetIdPermissions(int userId)
        {
            var list = await this.employeePermissionService.GetIdPermissionsOfUser(userId);
            return this.Ok(list);
        }

        /// <summary>
        /// Set permissions for a user
        /// </summary>
        /// <param name="permissionIds">Id of permissions</param>
        /// <param name="iduser">Id employee</param>
        /// <returns>Redirect to index page</returns>
        [HttpPost]
        [Permission(Action = "index", Controller = "permission")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetUserPermissions(int[] permissionIds, int iduser)
        {
            var message = string.Empty;
            try
            {
                await this.employeePermissionService.SetPermissionsForEmployee(
                    permissionIds ?? Enumerable.Empty<int>(),
                    iduser,
                    this.BaseContext.Account.UserId);

                message = "Cấp quyền cho tài khoản thành công";
            }
            catch (Exception ex)
            {
                message = "Error " + ex.Message;
            }

            this.SetSuccessAlert(message);
            return this.RedirectToAction("index");
        }

        /// <summary>
        /// Action: Set features to employee
        /// </summary>
        /// <returns>The page</returns>
        [HttpGet]
        [Permission(Action = "index", Controller = "permission")]
        public async Task<IActionResult> Features()
        {
            ViewBag.departmentIdCurrent = this.BaseContext.Account.DepartmentId;
            var features = await this.featureService.GetAllAsync();
            return this.View(features);
        }

        /// <summary>
        /// Get TreeNode features of the employee
        /// </summary>
        /// <param name="accountId">Employee id</param>
        /// <returns>TreeNode list</returns>
        [HttpGet]
        [Permission(Action = "index", Controller = "permission")]
        public async Task<IActionResult> GetTreeNodeFeaturesOfAccount(int accountId)
        {
            var list = await this.featureService.GetTreeNodeFeaturesOfAccount(accountId);
            return this.Ok(list);
        }

        /// <summary>
        /// Add features (beside permission) for account
        /// </summary>
        /// <param name="idFeatures">Id of features</param>
        /// <param name="idAccount">Id of account</param>
        /// <returns>Redirect to index page</returns>
        [HttpPost]
        [Permission(Action = "index", Controller = "permission")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetFeaturesForAccount(int[] idFeatures, int idAccount)
        {
            var message = string.Empty;
            try
            {
                await this.employeePermissionService.SetFeaturesForEmployee(
                idFeatures ?? Enumerable.Empty<int>(),
                idAccount,
                this.BaseContext.Account.EmployeeId);

                message = "Cấp thêm chức năng cho tài khoản thành công";
            }
            catch (Exception ex)
            {
                message = "Error " + ex.Message;
            }

            this.SetSuccessAlert(message);
            return this.RedirectToAction("Features");
        }
    }
}
