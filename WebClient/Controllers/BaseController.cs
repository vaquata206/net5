using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebClient.Contexts;
using WebClient.ViewModels;
using static WebClient.ViewModels.Alert;

namespace WebClient.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    ///
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// context factory
        /// </summary>
        private readonly IContextFactory contextFactory;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Status message
        /// </summary>
        private const string StatusMessage = "StatusMessage";

        /// <summary>
        /// A contrustor
        /// </summary>
        /// <param name="contextFactory">Context factory</param>
        /// <param name="logger">The Logger</param>
        public BaseController(IContextFactory contextFactory, ILogger logger)
        {
            this.contextFactory = contextFactory;
            this.logger = logger;
        }

        /// <summary>
        /// Get base context
        /// </summary>
        protected ApplicationContext BaseContext
        {
            get
            {
                return this.contextFactory.GetInstance();
            }
        }

        /// <summary>
        /// Get the logger
        /// </summary>
        protected ILogger Logger
        {
            get
            {
                return this.logger;
            }
        }

        /// <summary>
        /// Set alert
        /// </summary>
        /// <param name="message">Message of alert</param>
        /// <param name="alertTypes">Alert type</param>
        /// <param name="timer">timer</param>
        protected void SetAlert(string message, AlertType alertTypes, int timer = 3000)
        {
            var alert = new Alert
            {
                Type = alertTypes.GetHashCode(),
                Title = message,
                Timer = timer
            };
            this.TempData[StatusMessage] = JsonConvert.SerializeObject(alert);
        }

        /// <summary>
        /// Set a success alert
        /// </summary>
        /// <param name="message">Message Success</param>
        public void SetSuccessAlert(string message)
        {
            this.SetAlert(message, AlertType.Success);
        }

        /// <summary>
        /// Set a error alert
        /// </summary>
        /// <param name="message">Message error</param>
        public void SetErrorAlert(string message = null)
        {
            this.SetAlert(message ?? "Có lỗi xảy ra, xin vui lòng thử lại sau", AlertType.Error, 5000);
        }
    }
}
