using Microsoft.AspNetCore.Http;
using WebClient.Core;
using WebClient.Services.Interfaces;

namespace WebClient.Contexts
{
    /// <summary>
    /// Context factory
    /// </summary>
    public class ContextFactory : IContextFactory
    {
        /// <summary>
        /// A Application context
        /// </summary>
        private ApplicationContext applicationContext;

        /// <summary>
        /// Http context accessor
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Feature service
        /// </summary>
        private readonly IFeatureService featureService;

        private readonly AppSetting appSetting;

        /// <summary>
        /// A contrustor
        /// </summary>
        /// <param name="appSetting">App setting</param>
        /// <param name="httpContextAccessor">Http Context Accessor</param>
        /// <param name="featureService">Feature service</param>
        /// <param name="departmentService">department service</param>
        public ContextFactory(
            AppSetting appSetting,
            IHttpContextAccessor httpContextAccessor, 
            IFeatureService featureService)
        {
            this.appSetting = appSetting;
            this.httpContextAccessor = httpContextAccessor;
            this.featureService = featureService;
        }

        /// <summary>
        /// Get request context
        /// </summary>
        /// <returns>Request context</returns>
        public ApplicationContext GetInstance()
        {
            return this.GetApplicationContext();
        }

        /// <summary>
        /// Get a application context
        /// The context stored global information of a request
        /// </summary>
        /// <returns>A Feature context instance</returns>
        private ApplicationContext GetApplicationContext()
        {
            if (this.applicationContext == null)
            {
                this.applicationContext = new ApplicationContext(appSetting, this.httpContextAccessor, this.featureService);
            }

            return this.applicationContext;
        }
    }
}
