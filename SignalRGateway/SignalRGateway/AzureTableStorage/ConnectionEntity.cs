using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace SignalRGateway.AzureTableStorage
{
    public class ConnectionEntity : TableEntity
    {
        public ConnectionEntity() { }

        public ConnectionEntity(string userName, string connectionID)
        {
            this.PartitionKey = userName;
            this.RowKey = connectionID;
        }
    }
}