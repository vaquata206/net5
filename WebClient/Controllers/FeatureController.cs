using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClient.Attributes;
using WebClient.Contexts;
using WebClient.Core.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    public class FeatureController : BaseController
    {

        private readonly IFeatureService featureService;

        public FeatureController(
            IFeatureService featureService,
            ILogger<FeatureController> logger,
            IContextFactory contextFactory) : base(contextFactory, logger)
        {
            this.featureService = featureService;
        }

        /// <summary>
        /// Action index: show all feature of the system
        /// </summary>
        /// <returns>Index page</returns>
        [Permission]
        public async Task<IActionResult> Index()
        {
            var list = await this.featureService.GetAllAsync();
            return this.View(list);
        }

        /// <summary>
        /// Action saving feature
        /// </summary>
        /// <param name="featureVM">The feature</param>
        /// <returns>Redirect to index action</returns>
        [HttpPost]
        [Permission(Action = "index", Controller = "feature")]
        [ValidateAntiForgeryToken]
        [ValidateModelState(RedirectTo = "index")]
        public async Task<ActionResult> SaveFeature(FeatureVM featureVM)
        {
            try
            {
                await this.featureService.SaveFeatureAsync(featureVM);
                this.SetSuccessAlert(string.Format("Lưu chức năng {0} thành công", featureVM.Ten_CN));
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert();
            }
            
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Delete the feature
        /// </summary>
        /// <param name="featureId">Id of feature</param>
        /// <returns>The action</returns>
        [HttpDelete]
        [Permission(Action = "index")]
        public async Task<ActionResult> Delete(int featureId)
        {
            try
            {
                await this.featureService.DeleteFeatureAsync(featureId);
                this.SetSuccessAlert("Xóa chức năng thành công");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                this.SetErrorAlert();
            }

            return this.RedirectToAction("index");
        }
    }
}
