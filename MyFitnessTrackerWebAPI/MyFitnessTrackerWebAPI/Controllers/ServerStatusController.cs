using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyFitnessTrackerWebAPI.Models.Server;

namespace MyFitnessTrackerWebAPI.Controllers
{
    public class ServerStatusController : ApiController
    {
        // GET: api/ServerStatus
        public ServerStatus Get()
        {
            return new ServerStatus { LanguageCode = System.Threading.Thread.CurrentThread.CurrentCulture.Name };
        }

        
    }
}
