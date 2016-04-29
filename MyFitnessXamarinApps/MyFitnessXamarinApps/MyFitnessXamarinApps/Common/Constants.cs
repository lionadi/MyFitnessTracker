using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitnessXamarinApps.Common
{
    public class Constants
    {
        public static String Session_LogInuserId = "Cache_LogInuser.Id";

        public static String SignalR_HeaderID_Username = "username";
        public static String SignalR_HeaderID_Password = "password";

        public static string SignalR_HostApplicationUserName = "SignalRHostApplication";

        public static String UserLoginData_FileName = "UserLoginData.dat";
        public static String WebServiceLocation = "http://myfitnesstrackerwebapi.azurewebsites.net";
        public static String ServerExerciseRecordAttributeName_GEOLocation = "GEOLocation";
        //public static String WebServiceLocation = "http://192.168.163.188:52797";
        public static String SignalRGateway = "http://signalrgateway.azurewebsites.net/";
        //public static String SignalRGateway = "http://169.254.80.80:49212/";
        public static String SignalRHubName = "ChatHub";
    }
}
