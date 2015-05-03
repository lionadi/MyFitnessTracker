using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Client;
using System.Threading.Tasks;
using MyFitnessTrackerLibrary.ServiceBus;

namespace MyFitnessTrackerLibrary.SignalRLogic
{
    public class HubGateway
    {
        private String hubLocation = "http://signalrgateway.azurewebsites.net/";
        private static HubGateway _hubGateway = null;
        private String hubProxyName = "ChatHub";
        private IHubProxy hubProxy = null;
        private HubConnection hubConnection = null;
        private String sourceID = "NO ID";

        public IHubProxy HubProxyPoint
        {
            get { return this.hubProxy;  }
        }

        public String SourceID
        {
            get
            {
                return this.sourceID;
            }

            set
                
            {
                this.sourceID = value;
            }
            
        }

        public HubGateway()
        {
            hubConnection = new HubConnection(this.hubLocation);
            hubProxy = hubConnection.CreateHubProxy(hubProxyName);
        }

        ~HubGateway()
        {
            this.Stop();
        }

        public async Task SendNormalMessage(String name, String message)
        {
            await this.Start();
            await this.HubProxyPoint.Invoke("Send", name, message + " #Source ID: " + this.sourceID);
        }

        public async Task IsDataUpdateRequiredForWeb(String name, bool isRequired, String message)
        {
            await this.Start();
            await this.HubProxyPoint.Invoke("IsDataUpdateRequiredForWeb", name, isRequired, message + " #Source ID: " + this.sourceID);
            await NotificationGateway.GetInstance().SendMessage("New data was added. Your UI is updated/updating.");
        }

        public async Task IsDataUpdateRequiredForMobileClient(String name, bool isRequired, String message)
        {
            await this.Start();
            await this.HubProxyPoint.Invoke("IsDataUpdateRequiredForMobileClient", name, isRequired, message + " #Source ID: " + this.sourceID);
            await NotificationGateway.GetInstance().SendMessage("New data was added. Your UI is updated/updating.");
        }

        public static HubGateway GetInstance()
        {
            if( _hubGateway == null)
            {
                _hubGateway = new HubGateway();
            }

            return _hubGateway;
        }

        public async Task Start()
        {
            if(hubConnection.State == ConnectionState.Disconnected)
                await hubConnection.Start();
        }

        public void Stop()
        {
            hubConnection.Stop();
        }
    }
}