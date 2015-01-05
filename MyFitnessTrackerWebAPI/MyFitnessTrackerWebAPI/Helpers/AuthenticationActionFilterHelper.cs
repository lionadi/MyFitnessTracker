using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MyFitnessTrackerLibrary.Globals;
using MyFitnessTrackerWebAPI.Controllers;

namespace MyFitnessTrackerWebAPI.Helpers
{
    public class AuthenticationActionFilterHelper : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

        public override System.Threading.Tasks.Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, System.Threading.CancellationToken cancellationToken)
        {
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (SessionHelper.LoggedInUser<AspNetUser>(HttpContext.Current.User.Identity.Name) == null)
                {
                    //SessionHelper.UserSessionID = user.Id;
                    AspNetUsersController aspUserCon = new AspNetUsersController();
                    var sessionUser = aspUserCon.GetUser(HttpContext.Current.User.Identity.Name);
                    //SessionHelper.UserSessionID = user.UserName;
                    SessionHelper.LoggedInUser<AspNetUser>(sessionUser, sessionUser.UserName);
                }
            }
        }

        public override System.Threading.Tasks.Task OnActionExecutingAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}