using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClient.Contexts;

namespace WebClient.Areas.Api.Controllers
{
    /// <summary>
    /// Base api controller
    /// </summary>
    public abstract class BaseApiController : Controller
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
        /// A contrustor
        /// </summary>
        /// <param name="contextFactory">Context factory</param>
        /// <param name="logger">The Logger</param>
        public BaseApiController(IContextFactory contextFactory, ILogger logger)
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
    }
}
