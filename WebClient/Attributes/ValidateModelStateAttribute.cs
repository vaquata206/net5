using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using WebClient.Controllers;

namespace WebClient.Attributes
{
    /// <summary>
    /// Validate model state
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Redirect types
        /// </summary>
        public enum RedirectType
        {
            /// <summary>
            /// Type action
            /// </summary>
            Action,

            /// <summary>
            /// Type url
            /// </summary>
            URL
        }

        /// <summary>
        /// The message will returned if modelState isn't valid
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Will redirect to a "RedirectTo" if modelState isn't valid
        /// </summary>
        public string RedirectTo { get; set; }

        /// <summary>
        /// Redirect type
        /// </summary>
        public RedirectType Redirect { get; set; }

        /// <summary>
        /// On action executing
        /// </summary>
        /// <param name="context">Action executing context</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var controller = (BaseController)context.Controller;
                var message = string.IsNullOrEmpty(this.Message) ? 
                    string.Join(" | ", context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)) : 
                    this.Message;

                if (string.IsNullOrEmpty(this.RedirectTo))
                {
                    context.Result = controller.BadRequest(message);
                }
                else
                {
                    controller.SetErrorAlert(message);
                    context.Result = this.Redirect == RedirectType.URL ? controller.Redirect(this.RedirectTo) : controller.RedirectToAction(this.RedirectTo);
                }
            }
        }
    }
}
