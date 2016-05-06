using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using MyFitnessXamarinApps.Interfaces;
using MyFitnessXamarinApps.Common;

namespace MyFitnessXamarinApps.SignalR
{

	public class SignalRClientEventArgs : EventArgs
	{
		public readonly String Name;
		public readonly bool isRequired;
		public readonly String Message;

		public SignalRClientEventArgs(string name, bool isRequired, string message)
		{
			this.Name = name;
			this.isRequired = isRequired;
			this.Message = message;
		}

	}

    public class SignalRClient : ISignalRBase
    {
        private String hubLocation { get; set; }
        private ISignalRBase _hubGateway { get; set; }
        private String hubProxyName { get; set; }
        private IHubProxy hubProxy { get; set; }
        private HubConnection hubConnection { get; set; }
        private String sourceID { get; set; }
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

		public event EventHandler<SignalRClientEventArgs> IsDataUpdateRequiredForMobileClientEvent = delegate {
			
		};

        public IHubProxy HubProxyPoint
        {
            get { return this.hubProxy; }
        }

        public SignalRClient()
        {
			this.hubLocation = Constants.SignalRGateway;
			this.hubProxyName = Constants.SignalRHubName;
            hubConnection = new HubConnection(this.hubLocation);
            hubProxy = hubConnection.CreateHubProxy(hubProxyName);
        }

        ~SignalRClient()
        {
            this.Stop();
        }

        public ISignalRBase GetInstance()
        {
            if (_hubGateway == null)
            {
                _hubGateway = new SignalRClient();
            }

            return _hubGateway;
        }

        public async Task IsDataUpdateRequiredForMobileClient(string name, bool isRequired, string message)
        {
            await this.Start(name);
            await this.HubProxyPoint.Invoke("IsDataUpdateRequiredForMobileClient", name, isRequired, message + " #Source ID: " + this.sourceID);
            
        }

        public async Task IsDataUpdateRequiredForWeb(string name, bool isRequired, string message)
        {
            await this.Start(name);
            await this.HubProxyPoint.Invoke("IsDataUpdateRequiredForWeb", name, isRequired, message + " #Source ID: " + this.sourceID);
            
        }

        public async Task SendNormalMessage(string name, string message)
        {
            await this.Start(name);
            await this.HubProxyPoint.Invoke("Send", name, message + " #Source ID: " + this.sourceID);
        }

		public void AttachToSignalR()
		{
			this.HubProxyPoint.On<string, bool, string>("IsDataUpdateRequiredForMobileClient", (name, IsDataUpdateRequiredForWeb, message) => 
				//this.IsDataUpdateRequiredForMobileClientEvent(this, new SignalRClientEventArgs(name, IsDataUpdateRequiredForWeb, message)) 
				this.IsDataUpdateRequiredForMobileClientEvent.Invoke(this, new SignalRClientEventArgs(name, IsDataUpdateRequiredForWeb, message))
			);
		}

        public async Task Start(string userName)
        {
            if (hubConnection.State == ConnectionState.Disconnected)
            {
                if (!this.hubConnection.Headers.ContainsKey(Constants.SignalR_HeaderID_Username))
                    this.hubConnection.Headers.Add(new KeyValuePair<string, string>(Constants.SignalR_HeaderID_Username, userName));

                await hubConnection.Start();
            }
        }

        public void Stop()
        {
            hubConnection.Stop();
        }
    }
}
