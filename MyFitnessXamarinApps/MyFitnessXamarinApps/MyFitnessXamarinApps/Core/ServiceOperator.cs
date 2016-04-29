using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFitnessXamarinApps.Authentication;
using MyFitnessXamarinApps.AppData;
using MyFitnessXamarinApps.Common;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyFitnessXamarinApps.Core
{
    public class ServiceOperator
    {
        private UserLoginData LoginData { get; set; }
        private String serviceURL { get; set; }
        private String serviceAPIURL { get; set; }

        private WebApiHelper webApiHelper = null;

        private List<UserSetData> userSetsData = null;

        private static ServiceOperator serviceOperationSingleton = null;

        public static ServiceOperator GetInstance(String serviceURL)
        {
            if (serviceOperationSingleton == null)
            {
                serviceOperationSingleton = new ServiceOperator(serviceURL);
            }

            return serviceOperationSingleton;
        }

        public static ServiceOperator GetInstance()
        {
            if (serviceOperationSingleton == null)
            {
                serviceOperationSingleton = new ServiceOperator(Common.Constants.WebServiceLocation);
            }

            return serviceOperationSingleton;
        }

        public ServiceOperator(String serviceURL)
        {
            if(!String.IsNullOrEmpty(serviceURL))
            {
                this.LoginData = new UserLoginData();
                this.serviceURL = serviceURL;
                this.serviceAPIURL = serviceURL + "/api";
            }
        }

        public void LoginUser(String userName, String password)
        {
            this.LoginData.LoginToken = TokenAuthenticator.AuthenticateUser(this.serviceURL, password, userName);
            this.webApiHelper = new WebApiHelper(this.LoginData.LoginToken);
        }

        public void LoadUserData()
        {
            
            var userSetsDataServiceResults = this.webApiHelper.GetAsJSON(this.serviceAPIURL + "/sets");
            this.userSetsData = JsonConvert.DeserializeObject<List<UserSetData>>(userSetsDataServiceResults);
        }

        public List<UserSetData> GetUserSets()
        {
            return this.userSetsData;
        }

        public UserSetData GetUserSingleSet(int setID)
        {
            return this.userSetsData.FirstOrDefault(o => o.ID == setID.ToString());
        }

        public UserExerciseData GetUserSingleExcercise(int setID, int exerciseID)
        {
            var set = this.userSetsData.FirstOrDefault(o => o.ID == setID.ToString());

            if (set != null)
                return set.Exercises.FirstOrDefault(o => o.ID == exerciseID.ToString());
            return null;
        }
    }
}
