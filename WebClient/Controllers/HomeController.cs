using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClient.Contexts;
using WebClient.Core.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IBaoHongService baoHongService;

        /// <summary>
        /// Hom controller
        /// </summary>
        /// <param name="baoHongService">Bao hong service</param>
        /// <param name="logger">logger</param>
        /// <param name="contextFactory">Context factory</param>
        public HomeController(
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
        public IActionResult Index()
        {
            if (this.BaseContext.Account.IsKhachHang)
            {
                return Redirect("/baohong");
            }
            else
            {
                return Redirect("/home/quantri");
            }
        }

        public async Task<IActionResult> QuanTri()
        {
            var trangThai = await this.baoHongService.GetTrangThai();
            this.ViewBag.TrangThai = trangThai;
            return View();
        }

        public async Task<IActionResult> KiThuat()
        {
            var trangThai = await this.baoHongService.GetTrangThai();
            this.ViewBag.TrangThai = trangThai;
            return View();
        }

        public async Task<IActionResult> SearchAsync(BaoHongSearch search)
        {
            if (this.BaseContext.Account.IdVaiTro == 1)
            {
                search.IdKiThuatXuLy = this.BaseContext.Account.IdNhanVien;
            }
            var response = await this.baoHongService.SearchAsync(search);
            return this.Ok(response);
        }
    }
}
