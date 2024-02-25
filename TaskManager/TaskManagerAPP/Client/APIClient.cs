using TaskManagerAPP.Client.Request;
using TaskManagerAPP.Client.Requests;
using TaskManagerAPP.Client.Response;

namespace TaskManagerAPP.Client
{
    public class APIClient
    {
        private static string baseUri = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:443" : "https://localhost:443";

        public async Task<APIResponse<AccessTokenResponse>> LoginAsync(string email, string password)
        {
            var apiHttpClient = new APIHttpClient<LoginRequest, AccessTokenResponse>();

            var loginRequest = new LoginRequest();
            loginRequest.Email = email; 
            loginRequest.Password = password;            

            var postResponse = await apiHttpClient.PostAsync(baseUri + "/login", loginRequest);

            var response = ParseResponse<AccessTokenResponse>(postResponse);

            return response;
        }

        public async Task<APIResponse<bool>> RegisterAsync(string email, string password)
        {
            var apiHttpClient = new APIHttpClient<RegisterRequest, bool>();

            var registerRequest = new RegisterRequest();
            registerRequest.Email = email;
            registerRequest.Password = password;

            var postResponse = await apiHttpClient.PostAsync(baseUri + "/register", registerRequest);

            var response = ParseResponse<bool>(postResponse);

            return response;
        }

        public async Task<APIResponse<AccessTokenResponse>> RefreshTokenAsync(string refreshToken)
        {
            var apiHttpClient = new APIHttpClient<RefreshTokenRequest, AccessTokenResponse>();

            var refreshTokenRequest = new RefreshTokenRequest();            
            refreshTokenRequest.RefreshToken = refreshToken;

            var postResponse = await apiHttpClient.PostAsync(baseUri + "/refresh", refreshTokenRequest);

            var response = ParseResponse<AccessTokenResponse>(postResponse);

            return response;
        }

        private APIResponse<TAPIResponse> ParseResponse<TAPIResponse>(APIHttpClientResponse<TAPIResponse> httpClientResponse)
        {
            var response = new APIResponse<TAPIResponse>();

            response.Sucessfull = httpClientResponse.HttpStatusCode == System.Net.HttpStatusCode.OK ? true : false;
            response.Response = httpClientResponse.Response;
            response.ErrorDetails = httpClientResponse.ProblemDetails != null ? httpClientResponse.ProblemDetails.Errors?.Select(err => (err.Key, err.Value[0])).ToDictionary<string, string>() : null;

            return response;
        }
    }
}
