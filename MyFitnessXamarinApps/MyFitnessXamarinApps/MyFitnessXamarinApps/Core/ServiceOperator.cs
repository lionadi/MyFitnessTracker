using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFitnessXamarinApps.Authentication;
using MyFitnessXamarinApps.AppData;
using MyFitnessXamarinApps.Common;
using MyFitnessXamarinApps.SignalR;
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

		private SignalRClient signalRClient = null;

		public event EventHandler<SignalRClientEventArgs> IsDataUpdateRequiredForMobileClientEvent = delegate {};


        public static ServiceOperator GetInstance()
        {
            return serviceOperationSingleton;
        }

		public static void InitializeInstance(String userName)
		{
			if (serviceOperationSingleton == null)
			{
				serviceOperationSingleton = new ServiceOperator(Common.Constants.WebServiceLocation, userName);
			}
		}

		~ServiceOperator()
		{
			this.signalRClient.Stop ();
		}

		public async void StartSignalR()
		{
			await this.signalRClient.Start (this.LoginData.UserName);
			this.signalRClient.AttachToSignalR ();
		}


		public ServiceOperator(String serviceURL, String userName)
        {
			if(!String.IsNullOrEmpty(serviceURL) && !String.IsNullOrEmpty(userName))
            {
                this.LoginData = new UserLoginData();
				this.LoginData.UserName = userName;
                this.serviceURL = serviceURL;
                this.serviceAPIURL = serviceURL + "/api";
				this.signalRClient = new SignalRClient ();
				this.signalRClient.IsDataUpdateRequiredForMobileClientEvent += (object sender, SignalRClientEventArgs e) => { 
					//this.IsDataUpdateRequiredForMobileClientEvent( this, e);
					ServiceOperator.GetInstance().LoadUserData();
					this.IsDataUpdateRequiredForMobileClientEvent.Invoke(this, e);

				};
            }
        }

        public bool LoginUser(String userName, String password)
        {
			bool loginOK = false;
            this.LoginData.LoginToken = TokenAuthenticator.AuthenticateUser(this.serviceURL, password, userName);
            this.webApiHelper = new WebApiHelper(this.LoginData.LoginToken);
			if (this.LoginData.LoginToken != null && !String.IsNullOrEmpty (this.LoginData.LoginToken.AccessToken))
				loginOK = true;

			return loginOK;
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

        public void SaveUserActivityRecord(int setRowID, int excerciseRowID, double record)
        {
            var set = this.userSetsData[setRowID];
            var excercise = set.Exercises[excerciseRowID];
            var newExercise = new ExerciseRecord() { StartDate = DateTime.Now, EndDate = DateTime.Now, Date = DateTime.Now, ExerciseId = long.Parse(excercise.ID), Record = record };
            var newExerciseAsJSON = JsonConvert.SerializeObject(newExercise);
            var saveRecordServiceResults = this.webApiHelper.PostAsJSON(this.serviceAPIURL + "/ExerciseRecords", newExerciseAsJSON);
			var NewExerciseFromServerData = JsonConvert.DeserializeObject<ExerciseRecord>(saveRecordServiceResults);
			//this.userSetsData [setRowID].Exercises.Add (NewExerciseFromServerData);
        }
    }
}
