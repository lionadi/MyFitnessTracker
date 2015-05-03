using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Notifications;

namespace MyFitnessTrackerLibrary.ServiceBus
{
    // TODO: Replace this with a connection to the notification central hub, do not sent message directly from here in the future!!!!
    public class NotificationGateway
    {
        private NotificationHubClient hub = null;
        private static NotificationGateway _notificationGateway;
        public NotificationGateway()
        {

<<<<<<< HEAD
            hub = NotificationHubClient.CreateClientFromConnectionString(MyFitnessTrackerLibrary.Globals.MyFitAppSettings.NotificationHubConnectionString, MyFitnessTrackerLibrary.Globals.MyFitAppSettings.NotificationHubName);
=======
            hub = NotificationHubClient.CreateClientFromConnectionString("con string", "fittracker");
>>>>>>> origin/master
            
        
            
        }

        ~NotificationGateway()
        {

        }

        public async Task<NotificationOutcome> SendMessage(string message)
        {
            var toast = "{ \"data\" : {\"message\":\"" + "From : " + message + "\"}}";
            return await hub.SendGcmNativeNotificationAsync(toast);
        }

        public static NotificationGateway GetInstance()
        {
            if(_notificationGateway == null)
            {
                _notificationGateway = new NotificationGateway();
            }

            return _notificationGateway;
        }
    }
}
