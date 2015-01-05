using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using MyFitnessTrackerLibrary.Globals;
using MyFitnessTrackerVS.Controllers;

namespace MyFitnessTrackerVS.Helpers
{
    public class AuthenticationActionFilterHelper : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
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

    }
}