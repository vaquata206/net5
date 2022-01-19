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
    [Authorize]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService departmentService;

        /// <summary>
        /// Department controller
        /// </summary>
        /// <param name="departmentService">Department service</param>
        /// <param name="logger">logger</param>
        /// <param name="contextFactory">context factory</param>
        public DepartmentController(
            IDepartmentService departmentService,
            ILogger<AccountController> logger,
            IContextFactory contextFactory) : base(contextFactory, logger)
        {
            this.departmentService = departmentService;
        }

        /// <summary>
        /// The index of department
        /// </summary>
        /// <returns>the view index</returns>
        [Permission]
        public async Task<IActionResult> Index()
        {
            var currentUser = this.BaseContext.Account;
            var rs = await this.departmentService.GetTreeDepartmentsdWithTerm(currentUser.UserId, currentUser.DepartmentId);
            return this.View(rs);
        }

        /// <summary>
        /// detail department
        /// </summary>
        /// <param name="idDonVi">id don vi</param>
        /// <returns>the view</returns>
        [Permission(Action = "index")]
        public async Task<ActionResult> Detail(int idDonVi)
        {
            var department = await this.departmentService.GetByIdAsync(idDonVi);

            if (department == null)
            {
                return this.NotFound();
            }
            // handle selectbox don vi cha.
            var currentUser = this.BaseContext.Account;
            ViewBag.ListSelect = await this.departmentService.GetTreeDepartmentsdWithTerm(
                currentUser.UserId,
                currentUser.DepartmentId,
                idDonVi);

            return this.View(department);
        }

        /// <summary>
        /// Action saving feature
        /// </summary>
        /// <param name="departmentVM">The feature</param>
        /// <returns>Redirect to index action</returns>
        [HttpPost]
        [Permission(Action = "index")]
        [ValidateModelState(RedirectTo = "index")]
        public async Task<ActionResult> SaveDepartmentAsync(DepartmentVM departmentVM)
        {
            try
            {
                await this.departmentService.SaveAsync(departmentVM, this.BaseContext.Account.UserId);
                if (departmentVM.Id_DonVi == 0)
                {
                    this.SetSuccessAlert("Thêm mới đơn vị " + departmentVM.Ten_DonVi + " thành công.");
                    return this.RedirectToAction("Index");
                }
                else
                {
                    this.SetSuccessAlert("Cập nhập đơn vị " + departmentVM.Ten_DonVi + " thành công");
                    return this.Redirect("/Department/Detail?idDonVi=" + departmentVM.Id_DonVi);
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert("Có lỗi xảy ra, xin vui lòng thử lại sau");
                return this.RedirectToAction("Index");
            }
        }

        /// <summary>
        /// action update email of department
        /// </summary>
        /// <param name="emailDepartmentVM">the emailDepartment view model</param>
        /// <returns>the view detail </returns>
        [HttpPost]
        [Permission(Action = "index")]
        [ValidateModelState(RedirectTo = "index")]
        public async Task<IActionResult> UpdateEmail(EmailDepartmentVM emailDepartmentVM)
        {
            try
            {
                var account = this.BaseContext.Account;
                await this.departmentService.UpdateEmailAsync(
                    emailDepartmentVM,
                    account.UserId);

                this.SetSuccessAlert("Lưu cấu hình email thành công");
            }
            catch (Exception)
            {
                this.SetErrorAlert("Có lỗi xảy ra, xin vui lòng thử lại sau");
            }

            return this.Redirect("/Department/Detail?idDonVi=" + emailDepartmentVM.Id_DonVi);
        }

        /// <summary>
        /// Delete the department
        /// </summary>
        /// <param name="idDonVi">Id of department</param>
        /// <returns>The action</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission(Action = "index")]
        public async Task<IActionResult> Delete(int idDonVi)
        {
            try
            {
                await this.departmentService.DeleteAsync(idDonVi, this.BaseContext.Account.UserId);
                this.SetSuccessAlert("Xóa đơn vị thành công");
            }
            catch (Exception)
            {
                this.SetErrorAlert("Có lỗi xảy ra, xin vui lòng thử lại sau");
            }

            return RedirectToAction("index");
        }
    }
}
