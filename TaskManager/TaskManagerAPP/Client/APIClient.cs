using Newtonsoft.Json;
using System.Text;
using TaskManagerAPP.Client.Request;
using TaskManagerAPP.Client.Requests;
using TaskManagerAPP.Client.Response;

namespace TaskManagerAPP.Client
{
    public class APIClient
    {
        private static string baseUri = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:443" : "https://localhost:443";

        private readonly HttpClient httpClient;      

        public APIClient()
        {
            this.httpClient = CreateHttpClient();
        }

        private HttpClient CreateHttpClient()
        {
            HttpClient httpClient;

#if DEBUG
            httpClient = HttpClientFactory.Create(
                new HttpsClientHandlerService().GetPlatformMessageHandler(),
                new TokenRefreshHandler(this)
            );
#else
            httpClient = HttpClientFactory.Create(new TokenRefreshHandler(this));
#endif
            return httpClient;
        }

        public async Task<APIHttpClientResponse<TResponse>> PostAsync<TRequest, TResponse>(string requestUri, TRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var httpResponse = httpClient.PostAsync(requestUri, content).Result;

            string responseJson = await httpResponse.Content.ReadAsStringAsync();

            APIHttpClientResponse<TResponse> response = new APIHttpClientResponse<TResponse>();

            response.HttpStatusCode = httpResponse.StatusCode;

            try
            {
                httpResponse.EnsureSuccessStatusCode();
                response.Response = JsonConvert.DeserializeObject<TResponse>(responseJson);
            }
            catch (Exception ex)
            {
                // Response contains info about the actual issues
                response.ProblemDetails = JsonConvert.DeserializeObject<HttpValidationProblemDetails>(responseJson);
            }

            return response;
        }

        public async Task<APIResponse<AccessTokenResponse>> LoginAsync(string email, string password)
        {
            var loginRequest = new LoginRequest();
            loginRequest.Email = email; 
            loginRequest.Password = password;            

            var postResponse = await PostAsync<LoginRequest, AccessTokenResponse>(baseUri + "/login", loginRequest);

            var response = ParseResponse<AccessTokenResponse>(postResponse);

            return response;
        }

        public async Task<APIResponse<bool>> RegisterAsync(string email, string password)
        {
            var registerRequest = new RegisterRequest();
            registerRequest.Email = email;
            registerRequest.Password = password;

            var postResponse = await PostAsync<RegisterRequest, bool>(baseUri + "/register", registerRequest);

            var response = ParseResponse<bool>(postResponse);

            return response;
        }

        public async Task<APIResponse<AccessTokenResponse>> RefreshTokenAsync(string refreshToken)
        {
            var refreshTokenRequest = new RefreshTokenRequest();            
            refreshTokenRequest.RefreshToken = refreshToken;

            var postResponse = await PostAsync<RefreshTokenRequest, AccessTokenResponse>(baseUri + "/refresh", refreshTokenRequest);

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
