using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyFitnessTrackerVS;
using MyFitnessTrackerLibrary.Globals;

namespace MyFitnessTrackerVS.Controllers
{

    public class ControllerBase : Controller
    {
        protected AspNetUser user
        {
            get
            {
                if (User != null && User.Identity != null)
                    return (SessionHelper.LoggedInUser<AspNetUser>(User.Identity.Name));

                return (null);
            }
        }
    }
}