using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitnessTrackerLibrary.Globals
{
    public class MyFitAppSettings
    {
        public static String AzureTableStorageConnectionString
        {
            get
            {
                return Properties.Settings.Default.StorageConnectionString;
            }
        }

        public static String NotificationHubConnectionString
        {
            get
            {
                return Properties.Settings.Default.NotificationHubConnectionString;
            }
        }

        public static String NotificationHubName
        {
            get
            {
                return Properties.Settings.Default.NotificationHubName;
            }
        }

        public static String SignalRHubHostLocation
        {
            get
            {
                return Properties.Settings.Default.SignalRHubHostLocation;
            }
        }

        public static String SignalRHubProxy
        {
            get
            {
                return Properties.Settings.Default.SignalRHubProxy;
            }
        }
    }
}
