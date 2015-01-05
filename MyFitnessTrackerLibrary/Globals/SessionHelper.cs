using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFitnessTrackerLibrary.Globals
{
    public class SessionHelper
    {
        /// <summary>
        /// This needs to be set to use the properties in this session Helper
        /// </summary>
        public static String UserSessionID { get; set; }
        public static UserTemplate LoggedInUser<UserTemplate>()
        {
            if (!String.IsNullOrEmpty(UserSessionID))
            {
                String sessionID = Globals.Constants.Session_LogInUserID + UserSessionID;
                if (HttpContext.Current != null && HttpContext.Current.Cache[sessionID] is UserTemplate)
                {
                    return ((UserTemplate)HttpContext.Current.Cache[sessionID]);
                }
            } else
            {
                throw new NullReferenceException("Please provide a UNIQUE user session ID for caching!");
            }

            return (default(UserTemplate));
        }

        public static void LoggedInUser<UserTemplate>(UserTemplate value)
        {
            if (value != null && HttpContext.Current != null && HttpContext.Current.Cache != null && !String.IsNullOrEmpty(UserSessionID))
            {
                String sessionID = Globals.Constants.Session_LogInUserID + UserSessionID;
                HttpContext.Current.Cache[sessionID] = value;
            }
            else
            {
                throw new NullReferenceException("Please provide a UNIQUE user session ID for caching!");
            }
        }
    }
}