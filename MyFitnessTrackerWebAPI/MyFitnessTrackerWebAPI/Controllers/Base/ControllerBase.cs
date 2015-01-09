using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyFitnessTrackerWebAPI;
using MyFitnessTrackerLibrary.Globals;

namespace MyFitnessTrackerWebAPI.Controllers
{

    public class ControllerBase : ApiController
    {
        protected AspNetUser user { 
            get
            {
                if (User != null && User.Identity != null)
                    return (SessionHelper.LoggedInUser<AspNetUser>(User.Identity.Name));

                return (null);
            }
        }
    }
}