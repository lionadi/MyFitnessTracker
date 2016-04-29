using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace MyFitnessXamarinApps.Interfaces
{
    public interface ISignalRBase
    {
        Task SendNormalMessage(String name, String message);
        Task IsDataUpdateRequiredForWeb(String name, bool isRequired, String message);

        Task IsDataUpdateRequiredForMobileClient(String name, bool isRequired, String message);

        ISignalRBase GetInstance();

        Task Start(String userName);

        void Stop();
    }
}
