using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClient.Attributes;
using WebClient.Contexts;
using WebClient.Core.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    [Authorize]
    public class DotTiemVaccineController : BaseController
    {
        private readonly IDotTiemVaccineService dotTiemVaccineService;
        private readonly IDanhMucService danhMucService;

        /// <summary>
        /// Dot tiem vaccine controller
        /// </summary>
        /// <param name="dotTiemVaccineService">Dot tiem vaccine service</param>
        /// <param name="danhMucService">Danh muc service</param>
        /// <param name="logger">logger</param>
        /// <param name="contextFactory">Context factory</param>
        public DotTiemVaccineController(
            IDotTiemVaccineService dotTiemVaccineService,
            IDanhMucService danhMucService,
            ILogger<DotTiemVaccineController> logger,
            IContextFactory contextFactory) : base(contextFactory, logger)
        {
            this.dotTiemVaccineService = dotTiemVaccineService;
            this.danhMucService = danhMucService;
        }

        public IActionResult Index()
        {
            this.ViewBag.DepartmentId = BaseContext.Account.DepartmentId;
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        [Permission(Action = "index")]
        public async Task<IActionResult> Search(PagingRequest<DotTiemVaccineFilterVM> pagingRequest)
        {
            var response = await this.dotTiemVaccineService.SearchAsync(pagingRequest, this.BaseContext.Account.DepartmentId);
            return this.Ok(response);
        }

        [HttpGet]
        [ValidateModelState]
        [Permission(Action = "index")]
        public async Task<IActionResult> Xem(int id)
        {
            var thongTinDotTiem = await this.dotTiemVaccineService.LayThongTinDangKyTiemTheoIdDotTiem(id);
            return View(thongTinDotTiem);
        }

        [HttpGet]
        [ValidateModelState]
        [Permission(Action = "index")]
        public async Task<IActionResult> Chitiet(int id)
        {
            if (id > 0)
            {
                var thongTinDotTiem = await this.dotTiemVaccineService.LayThongTinDotTiemTheoId(id);
                return View(thongTinDotTiem);
            }
            else
            {
                return View(new DotTiemVaccineVM { Id_DonVi = this.BaseContext.Account.DepartmentId, Trang_Thai = 1 });
            }
        }

        [HttpGet]
        [ValidateModelState]
        [Permission(Action = "index")]
        public async Task<IActionResult> LayDsDotTiemConTheoIdCha(int id_Cha)
        {
            var dsDotTiemCon = await this.dotTiemVaccineService.LayDanhSachDotTiemVaccineTheoIdCha(id_Cha, this.BaseContext.Account.DepartmentId, this.BaseContext.Account.UserId);
            return this.Ok(dsDotTiemCon);
        }

        [HttpPost]
        [ValidateModelState]
        [Permission(Action = "index")]
        public async Task<IActionResult> Luu(DotTiemVaccineVM viewModel)
        {
            try
            {
                await this.dotTiemVaccineService.SaveAsync(viewModel, this.BaseContext.Account.UserId);
                var message = string.Empty;
                if (viewModel.Id_DotTiem == 0)
                {
                    message = "Thêm mới đợt tiêm " + viewModel.Ten_KeHoach + " thành công.";
                }
                else
                {
                    message = "Cập nhập đợt tiêm " + viewModel.Ten_KeHoach + " thành công.";
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

        /// <summary>
        /// Delete DotTiemVaccinne
        /// </summary>
        /// <param name="id">Id cua DotTiem</param>
        /// <returns>The action</returns>
        [HttpPost]
        [Permission(Action = "index")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await this.dotTiemVaccineService.DeleteByIdAsync(id, this.BaseContext.Account.UserId);
                this.SetSuccessAlert("Xóa đợt tiêm thành công");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert();
            }
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateModelState]
        [Permission(Action = "index")]
        public async Task<IActionResult> NhapKetQuaTiem(int id_DotTiem, IFormFile file)
        {
            try
            {
                await this.dotTiemVaccineService.NhapKetQuaTiem(id_DotTiem, file, this.BaseContext.Account);
                this.SetSuccessAlert("Đã lưu danh sách kết quả tiêm thành công!");
            }
            catch (Exception ex)
            {
                this.SetErrorAlert("Có lỗi xảy ra, xin vui lòng thử lại sau");
            }
            return this.Redirect("/DotTiemVaccine/Xem/" + id_DotTiem.ToString());
        }

        [HttpGet]
        [Permission(Action = "index")]
        public async Task<IActionResult> DangKy(int id)
        {
            var dottiemvaccine = await this.dotTiemVaccineService.LayThongTinDangKyTiemTheoIdDotTiem(id);
            var dsDoiTuongUuTien = await this.danhMucService.GetAllDmDoiTuongUuTienAsync();
            this.ViewBag.DoiTuongUuTien = dsDoiTuongUuTien;
            return this.View(dottiemvaccine);
        }

        /// <summary>
        /// Import danh sách đăng ký tiêm vaccine
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Permission(Action = "index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKyExcel(int id, IFormFile FileDSDKTiem)
        {
            try
            {
                await this.dotTiemVaccineService.DangKyExcel(id, FileDSDKTiem, this.BaseContext.Account);
                this.SetSuccessAlert("Lưu danh sách đăng ký tiêm thành công");
            }
            catch (Exception ex)
            {
                this.SetErrorAlert("Lưu danh sách thất bại. " + ex.Message);
            }
            return this.Redirect("/dottiemvaccine/dangky/" + id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportDsDangKyTiem(int id)
        {
            try
            {
                var excelFile = await this.dotTiemVaccineService.ExportDsDangKyTiem(id);
                return this.File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachDangKyTiemVacxin.xlsx");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert("Lấy dữ liệu không thành công");
                return this.Ok();
            }
        }

        [HttpPost]
        [Permission(Action = "index")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> LuuDanhSachDangKy(int id, string dangKyTiem, int dangKy)
        {
            try
            {
                var listDangKy = string.IsNullOrEmpty(dangKyTiem) ? Array.Empty<string>() : dangKyTiem.Split(';');
                await this.dotTiemVaccineService.LuuDanhSachTiemVaccine(id, listDangKy.Select(x => int.Parse(x)), dangKy, this.BaseContext.Account.UserId);
                SetSuccessAlert(string.Format("{0} danh sách tiêm vaccine thành công", dangKy == 1 ? "Đăng ký" : "Lưu"));
            }
            catch (Exception e)
            {
                this.Logger.LogError(e.Message);
                SetErrorAlert(string.Format("{0} danh sách tiêm vaccine không thành công", dangKy == 1 ? "Đăng ký" : "Lưu"));
            }

            return this.Redirect("/dottiemvaccine/dangky/" + id);
        }

        [HttpPost]
        [Permission(Action = "index")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> HuyDangKy(int id)
        {
            try
            {
                await this.dotTiemVaccineService.HuyDangKyDotTiemVaccine(id, this.BaseContext.Account);
                SetSuccessAlert("Hủy đăng ký đợt tiêm vaccine không thành công");
            }
            catch (Exception e)
            {
                this.Logger.LogError(e.Message);
                SetErrorAlert("Hủy đăng ký đợt tiêm vaccine không thành công");
            }

            return this.Redirect("/dottiemvaccine/dangky/" + id);
        }

        /// <summary>
        /// Xóa: Cập nhật tình trạng người đăng ký tiêm vaccine
        /// </summary>
        /// <param name="id">id người bị xóa</param>
        /// <returns>trang danh sách người đk</returns>
        [HttpPost]
        [Permission(Action = "index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChotDanhSachDotTiem(int id)
        {
            try
            {
                await this.dotTiemVaccineService.ChotDanhSachDotTiem(id, this.BaseContext.Account);
                this.SetSuccessAlert("Chốt danh sách đợt tiêm thành công");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert("Chốt danh sách đợt tiêm thất bại");
            }

            return this.Redirect("/DotTiemVaccine/Xem/" + id);

        }
    }
} 
