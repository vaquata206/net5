using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClient.Attributes;
using WebClient.Contexts;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    [Authorize]
    public class BaoHongController : BaseController
    {
        private readonly IBaoHongService baoHongService;

        /// <summary>
        /// Baohong controller
        /// </summary>
        /// <param name="baoHongService">Bao hong service</param>
        /// <param name="logger">logger</param>
        /// <param name="contextFactory">Context factory</param>
        public BaoHongController(
            IBaoHongService baoHongService,
            ILogger<HomeController> logger,
            IContextFactory contextFactory) : base(contextFactory, logger)
        {
            this.baoHongService = baoHongService;
        }

        /// <summary>
        /// Home page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> IndexAsync()
        {
            var trangThai = await this.baoHongService.GetTrangThai();
            this.ViewBag.TrangThai = trangThai;
            return View();
        }

        public async Task<IActionResult> SearchAsync(BaoHongSearch search)
        {
            search.IdKhachHang = this.BaseContext.Account.IdKhachHang;
            var response = await this.baoHongService.SearchAsync(search);
            return this.Ok(response);
        }

        public async Task<IActionResult> ChiTiet(int id)
        {
            var entity = await this.baoHongService.GetByIdAsync(id);
            DichVuKhachHang dichVuKhachHang = null;
            IEnumerable<ChiTietPhieuBaoHong> chiTiet;
            if (entity == null)
            {
                dichVuKhachHang = new DichVuKhachHang();
                entity = new PhieuBaoHong();
                chiTiet = new List<ChiTietPhieuBaoHong>();
            }
            else {
                dichVuKhachHang = await this.baoHongService.GetDichVuKhachHangByIdAsync(entity.IdDichVuKhachHang);
                chiTiet = await this.baoHongService.GetChiTietBaoHong(entity.Id);
            }
            var dichVu = await this.baoHongService.GetDichVuByKhachHangId(dichVuKhachHang.Id == 0 ? this.BaseContext.Account.IdKhachHang.Value: dichVuKhachHang.IdKhachHang);

            this.ViewBag.KhachHang = await this.baoHongService.GetKhachHang(dichVuKhachHang.Id == 0 ? this.BaseContext.Account.IdKhachHang.Value : dichVuKhachHang.IdKhachHang);
            this.ViewBag.DichVuKhachHang = dichVuKhachHang;
            this.ViewBag.DichVu = dichVu;
            this.ViewBag.Account = this.BaseContext.Account;
            this.ViewBag.NVKyThuat = await this.baoHongService.GetNhanVienKyThuat();
            this.ViewBag.ChiTiet = chiTiet;
            return this.View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateModelState]
        public async Task<IActionResult> Save(BaoHongVM viewModal)
        {
            await this.baoHongService.SaveAsync(viewModal, this.BaseContext.Account.IdKhachHang.Value);
            return this.Redirect("/baohong");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TiepNhan(int id)
        {
            await this.baoHongService.TiepNhan(id, this.BaseContext.Account.IdNhanVien.Value);
            this.SetSuccessAlert("Tiếp nhận thành công");
            return this.Redirect("/baohong/chitiet/" + id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChuyenKyThuat(ChuyenKyThuatVM viewModal)
        {
            await this.baoHongService.ChuyenKyThuat(viewModal);
            this.SetSuccessAlert("Chuyển kĩ thuật thành công");
            return this.Redirect("/baohong/chitiet/" + viewModal.Id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HoanThanh(int id)
        {
            await this.baoHongService.HoanThanh(id, this.BaseContext.Account.IdNhanVien.Value);
            this.SetSuccessAlert("Báo hoàn thành thành công");
            return this.Redirect("/baohong/chitiet/" + id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DanhGia(DanhGiaVM danhGiaVM)
        {
            await this.baoHongService.GuiDanhGia(danhGiaVM);
            this.SetSuccessAlert("Báo hoàn thành thành công");
            return this.Redirect("/baohong/chitiet/" + danhGiaVM.Id);
        }
    }
}
