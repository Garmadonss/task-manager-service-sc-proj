using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPP.Client;
using TaskManagerAPP.Client.NewFolder;
using TaskManagerAPP.Client.Requests;

namespace TaskManagerAPP.Client
{
    public class APIClient
    {
        private static string baseUri = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:443" : "https://localhost:443";

        public async Task<AccessTokenResponse?> LoginAsync(string email, string password)
        {
            var apiHttpClient = new APIHttpClient<LoginRequest, AccessTokenResponse>();

            var loginRequest = new LoginRequest();
            loginRequest.Email = email; 
            loginRequest.Password = password;            

            var response = await apiHttpClient.PostAsync(baseUri + "/login", loginRequest);            

            return response;
        }

    }
}
