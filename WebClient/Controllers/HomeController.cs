using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.ViewModels;
using WebClient.Contexts;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IDangKyTiemVaccineService dangKyTiemVaccineService;

        /// <summary>
        /// Hom controller
        /// </summary>
        /// <param name="dangKyTiemVaccineService">Dang ky tiem service</param>
        /// <param name="logger">logger</param>
        /// <param name="contextFactory">Context factory</param>
        public HomeController(
            IDangKyTiemVaccineService dangKyTiemVaccineService,
            ILogger<HomeController> logger, 
            IContextFactory contextFactory) : base (contextFactory, logger)
        {
            this.dangKyTiemVaccineService = dangKyTiemVaccineService;
        }

        public async Task<IActionResult> Index()
        {
            var thongKe = await this.dangKyTiemVaccineService.ThongKeTongQuatSoLuong(this.BaseContext.Account.DepartmentId);
            return View(thongKe);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
