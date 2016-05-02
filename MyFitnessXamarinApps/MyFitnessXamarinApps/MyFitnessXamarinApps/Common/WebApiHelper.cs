using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFitnessXamarinApps.AppData;
using MyFitnessXamarinApps.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyFitnessXamarinApps.Common
{
    public class WebApiHelper
    {
        private Token userLoginToken { get; set; }

        public WebApiHelper(Token userLoginToken)
        {
            this.userLoginToken = userLoginToken;
        }

        public String GetAsJSON(String url)
        {

            String result = null;
            try
            {


                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.userLoginToken.AccessToken);
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                //requestMessage.Headers.Add("Accept", "application/json");
                //requestMessage.Headers.Add("Content-Type", "application/json");
                var response = client.SendAsync(requestMessage);
                result = response.Result.Content.ReadAsStringAsync().Result;
            } catch(Exception ex)
            {
                throw;
            }

            return result;
        }

        public String PostAsJSON(String URL, String dataAsJSON)
        {
            String result = null;
            try
            {


                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.userLoginToken.AccessToken);
                
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, URL);
                //requestMessage.Headers.Add("Accept", "application/json");
                //requestMessage.Headers.Add("Content-Type", "application/json");


                var response = client.PostAsync(URL, new StringContent(dataAsJSON, Encoding.UTF8, "application/json"));
                result = response.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;

        }
    }
}
