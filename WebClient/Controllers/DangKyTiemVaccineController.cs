using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Attributes;
using WebClient.Contexts;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    public class DangKyTiemVaccineController : BaseController
    {
        private readonly IDangKyTiemVaccineService dangKyTiemVaccineService;
        private readonly IDanhMucService danhMucService;
        private readonly IDepartmentService departmentService;
        private readonly ILichSuTiemService lichSuTiemService;

        /// <summary>
        /// Dang ky tiem vaccine controller
        /// </summary>
        /// <param name="dangKyTiemVaccineService">Dang ky tiem service</param>
        /// <param name="danhMucService">danh muc service</param>
        /// <param name="departmentService">don vi service</param>
        /// <param name="lichSuTiemService">lich su tiem service</param>
        /// <param name="logger">logger</param>
        /// <param name="contextFactory">Context factory</param>
        public DangKyTiemVaccineController(
            IDangKyTiemVaccineService dangKyTiemVaccineService,
            IDanhMucService danhMucService,
            IDepartmentService departmentService,
            ILichSuTiemService lichSuTiemService,
            ILogger<DangKyTiemVaccineController> logger,
            IContextFactory contextFactory) : base(contextFactory, logger)
        {
            this.dangKyTiemVaccineService = dangKyTiemVaccineService;
            this.danhMucService = danhMucService;
            this.departmentService = departmentService;
            this.lichSuTiemService = lichSuTiemService;
        }

        [Permission]
        public async Task<IActionResult> Index()
        {
            try
            {
                var dsDoiTuongUuTien = await this.danhMucService.GetAllDmDoiTuongUuTienAsync();
                var department = await this.departmentService.GetTreeDepartmentsdWithTerm(this.BaseContext.Account.UserId, this.BaseContext.Account.DepartmentId);
                this.ViewBag.DsDoiTuongUuTien = dsDoiTuongUuTien;
                this.ViewBag.Departments = department;
                return View();
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert(ex.Message);
                return this.RedirectToAction("index");
            }
        }

        /// <summary>
        /// Tìm kiếm đối tượng đăng ký tiêm chủng
        /// </summary>
        /// <param name="pagingRequest">Paging request</param>
        /// <returns>Danh sách đối tượng đăng ký tiêm</returns>
        [HttpPost]
        [ValidateModelState]
        [Permission(Action = "index")]
        public async Task<IActionResult> Search(PagingRequest<DoiTuongDangKyTiemFilterVM> pagingRequest)
        {
            try
            {
                if (pagingRequest.Filter.IdDonVi == 0)
                {
                    pagingRequest.Filter.IdDonVi = this.BaseContext.Account.DepartmentId;
                }

                if (pagingRequest.Filter.DsDoiTuongUuTien != null)
                {
                    pagingRequest.Filter.DsDoiTuongUuTien = pagingRequest.Filter.DsDoiTuongUuTien.Where(x => x != 0).ToArray();
                }

                var response = await this.dangKyTiemVaccineService.SearchAsync(pagingRequest);
                return this.Ok(response);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Chi tiết người đăng ký tiêm vaccine
        /// </summary>
        /// <param name="id">Id người đăng ký</param>
        /// <returns>Chi tiết thông tin người đăng ký</returns>
        [HttpGet]
        [Permission(Action = "index")]
        public async Task<IActionResult> ChiTiet(int id)
        {
            try
            {
                var currentUser = this.BaseContext.Account;
                var thongTin = await this.dangKyTiemVaccineService.GetByIdASync(id);
                var dsDoiTuongUuTien = await this.danhMucService.GetAllDmDoiTuongUuTienAsync();
                this.ViewBag.DsDoiTuongUuTien = dsDoiTuongUuTien;
                var dsDonVi = await this.departmentService.GetTreeDepartmentsdWithTerm(
                   currentUser.UserId,
                   currentUser.DepartmentId);
                this.ViewBag.DsDonVi = dsDonVi;
                var dsDanToc = await this.danhMucService.GetAllDmDanTocAsync();
                this.ViewBag.DsDanToc = dsDanToc;
                var dsLichSuTiem = await this.lichSuTiemService.LayDsLichSuTiemTheoIdNguoiDangKy(id);
                this.ViewBag.DsLichSuTiem = dsLichSuTiem;
                return this.View(thongTin);
            }
            catch(Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert("Lấy thông tin chi tiết thất bại. Vui lòng kiểm tra lại.");
                return RedirectToAction("index");
            }
        }

        /// <summary>
        /// Cập nhật thông tin người đăng ký tiêm vaccine
        /// </summary>
        /// <param name="thongTinNguoiDanVM">thông tin cập nhật</param>
        /// <returns>trang chi tiết</returns>
        [HttpPost]
        [Permission(Action = "index")]
        [ValidateAntiForgeryToken]
        [ValidateModelState]
        public async Task<IActionResult> Save(ThongTinNguoiDanVM thongTinNguoiDanVM)
        {
            try
            {
                await this.dangKyTiemVaccineService.SaveAsync(thongTinNguoiDanVM, this.BaseContext.Account);
                this.SetSuccessAlert("Cập nhật thông tin thành công");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert("Cập nhật thông tin thất bại. Vui lòng kiểm tra lại.");
            }

            return this.Redirect(string.Format("/dangkytiemvaccine/chitiet/{0}", thongTinNguoiDanVM.Id_ThongTin));

        }

        /// <summary>
        /// Xóa: Cập nhật tình trạng người đăng ký tiêm vaccine
        /// </summary>
        /// <param name="id">id người bị xóa</param>
        /// <returns>trang danh sách người đk</returns>
        [HttpPost]
        [Permission(Action = "index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this.dangKyTiemVaccineService.DeleteAsync(id, this.BaseContext.Account);
                this.SetSuccessAlert("Xóa thông tin thành công");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert("Xóa thông tin thất bại");
                return this.Redirect(string.Format("/dangkytiemvaccine/chitiet/{0}", id));
            }

            return this.RedirectToAction("index");
        }

        /// <summary>
        /// Import danh sách đăng ký tiêm vaccine
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Permission(Action = "index")]
        [ValidateAntiForgeryToken]
        [ValidateModelState]
        public async Task<IActionResult> DongBoDSDKTiemFromExcel(IFormFile FileDSDKTiem)
        {
            try
            {
                await this.dangKyTiemVaccineService.DongBoDSDKTiemFromExcel(FileDSDKTiem, this.BaseContext.Account);
                this.SetSuccessAlert("Thêm mới danh sách đăng ký tiêm thành công");
            }
            catch (Exception ex)
            {
                this.SetErrorAlert("Thêm mới thất bại. " + ex.Message);
            }
            return this.RedirectToAction("index");
        }
    }
}
