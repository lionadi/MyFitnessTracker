using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyFitnessXamarinApps.Authentication
{
    public class TokenAuthenticator
    {
        public static Token AuthenticateUser(String URL, String password, String userName)
        {
            Token token = null;

            var client = new HttpClient();
            var authorizationHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes("rajeev:secretKey"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeader);

            var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", userName},
                   {"password", password},
               };

            var tokenResponse = client.PostAsync(URL + "/token", new FormUrlEncodedContent(form)).Result;
            token = JsonConvert.DeserializeObject<Token>(tokenResponse.Content.ReadAsStringAsync().Result);

            return token;
        }
    }
}
