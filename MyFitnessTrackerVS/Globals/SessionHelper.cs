using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFitnessTrackerVS.Globals
{
    public class SessionHelper
    {
        public static AspNetUser LoggedInUser
        {
            get
            {
                return(HttpContext.Current.Session[Globals.Constants.Session_LogInUserID] as AspNetUser);
            }

            set
            {
                if(value != null)
                    HttpContext.Current.Session.Add(Globals.Constants.Session_LogInUserID, value);
            }
        }
    }
}