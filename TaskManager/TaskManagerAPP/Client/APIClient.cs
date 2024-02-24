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

            return response.Response;
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            var apiHttpClient = new APIHttpClient<RegisterRequest, bool>();

            var registerRequest = new RegisterRequest();
            registerRequest.Email = email;
            registerRequest.Password = password;

            var response = await apiHttpClient.PostAsync(baseUri + "/register", registerRequest);

            return response.Response;
        }

        private void ValidateResponse()
        {

        }
    }
}
