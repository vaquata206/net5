using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebClient.Contexts;

namespace WebClient.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Controller { get; set; }
        public string Action { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                // it isn't needed to set unauthorized result 
                // as the base class already requires the user to be authenticated
                // this also makes redirect to a login page work properly
                // context.Result = new UnauthorizedResult();
                return;
            }

            if (string.IsNullOrEmpty(this.Controller))
            {
                this.Controller = context.RouteData.Values["controller"].ToString().ToLower();
            }

            if (string.IsNullOrEmpty(this.Action))
            {
                this.Action = context.RouteData.Values["action"].ToString().ToLower();
            }

            var contextFactory = (IContextFactory)context.HttpContext.RequestServices.GetService(typeof(IContextFactory));
            var baseContext = contextFactory.GetInstance();
            var isAuthorized = baseContext.Menu.Any(x => this.Controller.Equals(x.Controler) && this.Action.Equals(string.IsNullOrEmpty(x.Action) ? "index" : x.Action));
            if (!isAuthorized)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }
        }
    }
}
