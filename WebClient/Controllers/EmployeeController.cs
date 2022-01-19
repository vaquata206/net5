using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClient.Attributes;
using WebClient.Contexts;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    public class EmployeeController : BaseController
    {
        /// <summary>
        /// The employee service
        /// </summary>
        private readonly IEmployeeService employeeService;

        /// <summary>
        /// the interface departmentService
        /// </summary>
        private readonly IDepartmentService departmentService;

        /// <summary>
        /// Account service
        /// </summary>
        private readonly IAccountService accountService;

        public EmployeeController(
            IAccountService accountService,
            IEmployeeService employeeService,
            IDepartmentService departmentService,
            ILogger<AccountController> logger,
            IContextFactory contextFactory) : base(contextFactory, logger)
        {
            this.accountService = accountService;
            this.employeeService = employeeService;
            this.departmentService = departmentService;
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>View Index</returns>
        [Permission]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = this.BaseContext.Account;
            var lst = await this.departmentService.GetTreeDepartmentsdWithTerm(currentUser.UserId, currentUser.DepartmentId);
            return View(lst);
        }

        /// <summary>
        /// Detail employee
        /// </summary>
        /// <param name="id">the idNhanVien</param>
        /// <returns>View detail</returns>
        [HttpGet]
        [Permission(Action = "index")]
        public async Task<IActionResult> Detail(int id)
        {
            var emp = await this.employeeService.GetByIdAsync(id);

            if (emp == null)
            {
                return this.NotFound();
            }

            var department = await this.departmentService.GetByIdAsync(emp.Id_DonVi);
            if (department == null)
            {
                return this.NotFound();
            }

            var accounts = await this.accountService.GetAccountsByEmployeeId(id);

            ViewBag.Accounts = accounts;
            ViewBag.DepartmentName = department.Ten_DonVi;
            return this.View(emp);
        }

        /// <summary>
        /// action get json employees by departmentId with page request
        /// </summary>
        /// <param name="request">page request</param>
        /// <returns>json result</returns>
        [HttpPost]
        [Permission(Action = "index")]
        [ValidateModelState]
        public async Task<IActionResult> GetEmployeesByDeparmentId(PagingRequest<string> request)
        {
            var lstEmp = await this.employeeService.GetEmployeesByDeparmentIdAsync(int.Parse(request.Filter, null));
            var data = lstEmp.Skip(request.Start).Take(request.Length).ToList();
            return this.Ok(new PagingResponse<Employee>()
            {
                Data = data,
                Draw = request.Draw,
                RecordsFiltered = lstEmp.Count(),
                RecordsTotal = lstEmp.Count(),
            });
        }
    
        /// <summary>
        /// Delete a employee
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>redirect to index page</returns>
        [HttpPost]
        [Permission(Action = "index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int employeeId)
        {
            try
            {
                await this.employeeService.DeleteByIdAsync(employeeId, this.BaseContext.Account.UserId);
                this.SetSuccessAlert("Xóa nhân viên thành công");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert();
            }

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// save employee
        /// </summary>
        /// <param name="employeeVM">the employeeVM</param>
        /// <returns>the View</returns>
        [HttpPost]
        [Permission(Action = "index")]
        [ValidateModelState(RedirectTo = "index")]
        public async Task<ActionResult> SaveEmployee(EmployeeVM employeeVM)
        {
            try
            {
                await this.employeeService.SaveAsync(employeeVM, this.BaseContext.Account.UserId);
                var message = string.Empty;
                if (employeeVM.Id_NhanVien == 0)
                {
                    message = "Thêm mới nhân viên " + employeeVM.HoTen + " thành công.";
                }
                else
                {
                    message = "Cập nhập nhân viên " + employeeVM.HoTen + " thành công";
                }

                this.SetSuccessAlert(message);
            }
            catch (Exception ex)
            {
                this.SetErrorAlert();
                this.Logger.LogError(ex.Message);
            }

            return this.RedirectToAction("Index");
        }
    }
}
