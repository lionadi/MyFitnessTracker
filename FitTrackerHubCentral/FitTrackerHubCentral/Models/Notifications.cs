using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ServiceBus.Notifications;

namespace FitTrackerHubCentral.Models
{
    public class Notifications
    {
        public static Notifications Instance = new Notifications();

        public NotificationHubClient Hub { get; set; }

        private Notifications()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString(MyFitnessTrackerLibrary.Globals.MyFitAppSettings.NotificationHubConnectionString, MyFitnessTrackerLibrary.Globals.MyFitAppSettings.NotificationHubName);
        }
		
		
    }
}