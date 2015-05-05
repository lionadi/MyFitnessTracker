using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SignalRGateway.AzureTableStorage;
using System.Configuration;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using MyFitnessTrackerLibrary.Globals;

namespace SignalRGateway
{
    public class ChatHub : Hub
    {
        //protected override void Dispose(bool disposing)
        //{
        //    // Get all of the SignalR host application connections to clear them from the table when the application quits
        //    var queryResult = this.SendMessageTo(Constants.SignalR_HostApplicationUserName.ToLowerInvariant(), "Deleting");

        //    var table = GetConnectionTable();
        //    foreach (var entity in queryResult)
        //    {
        //        var deleteOperation = TableOperation.Delete(
        //        new ConnectionEntity(Constants.SignalR_HostApplicationUserName.ToLowerInvariant(), Context.ConnectionId) { ETag = "*" });
        //        table.Execute(deleteOperation);
        //    }
            
        //    base.Dispose(disposing);
        //}
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            var queryResult = this.SendMessageTo(name, message);
            foreach (var entity in queryResult)
            {
                Clients.Client(entity.RowKey).broadcastMessage(name, message);
            }
        }

        public void IsDataUpdateRequiredForWeb(string name, bool isRequired, string message)
        {
            var queryResult = this.SendMessageTo(name, message);
            foreach (var entity in queryResult)
            {
                Clients.Client(entity.RowKey).isDataUpdateRequiredForWeb(name, isRequired, message);
            }
            Clients.All.isDataUpdateRequiredForWeb(name, isRequired, message);
        }

        public void IsDataUpdateRequiredForMobileClient(string name, bool isRequired, string message)
        {
            var queryResult = this.SendMessageTo(name, message);
            foreach (var entity in queryResult)
            {
                Clients.Client(entity.RowKey).isDataUpdateRequiredForMobileClient(name, isRequired, message);
            }
        }

        private List<ConnectionEntity> SendMessageTo(String who, String message)
        {
            //var name = Context.User.Identity.Name;
            var name = this.GetConnectionUser();

            if (!String.IsNullOrEmpty(name))
            {
                var table = GetConnectionTable();

                // Notice that the partition keys are stored in azure storage as lower case
                var query = new TableQuery<ConnectionEntity>()
                    .Where(TableQuery.GenerateFilterCondition(
                    "PartitionKey",
                    QueryComparisons.Equal,
                    who.ToLowerInvariant()));

                var queryResult = table.ExecuteQuery(query).ToList();
                if (queryResult.Count == 0)
                {
                    Clients.Caller.showErrorMessage("The user is no longer connected.");
                }
                else
                {
                    // Load only once the host application connections to display the data there
                    if(queryResult.Count(o=>o.PartitionKey.Equals(Constants.SignalR_HostApplicationUserName.ToLowerInvariant())) <= 0)
                        queryResult.AddRange(this.SendMessageTo(Constants.SignalR_HostApplicationUserName, message));

                    return queryResult;
                }
            }

            return new List<ConnectionEntity>();
        }

        // This assumes that "normmaly" all others clients than the host SignalR web application (this app) will use header named as username for user identification. The SignalR web app will user querystring.
        private String GetConnectionUser()
        {
            var name = Context.Headers[Constants.SignalR_HeaderID_Username];

            if (String.IsNullOrEmpty(name))
            {
                name = Context.QueryString[Constants.SignalR_HeaderID_Username];
            }
            if (String.IsNullOrEmpty(name))
                return null;

            // Notice that the partition keys are stored in azure storage as lower case
            return name.ToLowerInvariant();
        }

        public override Task OnConnected()
        {
            //var name = Context.User.Identity.Name;
            var name = this.GetConnectionUser();
            
            if(!String.IsNullOrEmpty(name))
            {
                var table = GetConnectionTable();
                var created = table.CreateIfNotExists();

                var entity = new ConnectionEntity(
                    name.ToLower(),
                    Context.ConnectionId);
                var insertOperation = TableOperation.InsertOrReplace(entity);
                table.Execute(insertOperation);
            }
            

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //var name = Context.User.Identity.Name;
            var name = this.GetConnectionUser();

            if (!String.IsNullOrEmpty(name))
            {
                var table = GetConnectionTable();

                var deleteOperation = TableOperation.Delete(
                    new ConnectionEntity(name, Context.ConnectionId) { ETag = "*" });
                table.Execute(deleteOperation);
            }

            return base.OnDisconnected(stopCalled);
        }

        private CloudTable GetConnectionTable()
        {
            
            var storageAccount =
                CloudStorageAccount.Parse(
                MyFitnessTrackerLibrary.Globals.MyFitAppSettings.AzureTableStorageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("connection");
            
            return table;
        }
    }
}