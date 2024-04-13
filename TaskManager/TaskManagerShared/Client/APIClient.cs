using Azure.Core;
using Newtonsoft.Json;
using System;
using System.Text;
using TaskManagerShared.Client.Request;
using TaskManagerShared.Client.Requests;
using TaskManagerShared.Client.Response;
using Xamarin.Essentials;

namespace TaskManagerShared.Client
{
    public class APIClient
    {
        private string baseUri;

        private readonly HttpClient httpClient;      

        public APIClient(IBearerTokenProvider bearerTokenProvider, HttpMessageHandler httpMessageHandler, string baseUri)
        {
            this.httpClient = CreateHttpClient(bearerTokenProvider, httpMessageHandler);
            if (baseUri.StartsWith("https://localhost"))
            {
                this.baseUri = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:443" : "https://localhost:443";
            }
            else
            {
                this.baseUri = baseUri;
            }            
        }

        private HttpClient CreateHttpClient(IBearerTokenProvider bearerTokenProvider, HttpMessageHandler httpMessageHandler)
        {
            HttpClient httpClient;

#if DEBUG
            httpClient = HttpClientFactory.Create(
                httpMessageHandler,
                new TokenRefreshHandler(this, bearerTokenProvider)
            );
#else
            httpClient = HttpClientFactory.Create(new TokenRefreshHandler(this, bearerTokenProvider));
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

        public async Task<APIHttpClientResponse<TResponse>> GetAsync<TResponse>(string requestUri, Dictionary<string, string> parameters)
        {
            var requestUriWithParameters = requestUri + "?" + String.Join("&", parameters.Select(p => p.Key + "=" + p.Value).ToList());

            var httpResponse = httpClient.GetAsync(requestUriWithParameters).Result;

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

        public async Task<APIResponse<bool>> AddTaskAsync(string title, string description, DateTime deadline, TaskPriority priority)
        {
            var request = new AddTaskRequest()
            {
                Title = title,
                Description = description,
                Deadline = deadline,
                Priority = priority
            };

            var postResponse = await PostAsync<AddTaskRequest, bool>(baseUri + "/task" + "/add", request);

            var response = ParseResponse<bool>(postResponse);

            return response;
        }

        public async Task<APIResponse<bool>> EditTaskAsync(int id, string title, string description, bool done, DateTime deadline, TaskPriority priority)
        {
            var request = new EditTaskRequest()
            {
                Id = id,
                Title = title,
                Description = description,
                Done = done,
                Deadline = deadline,
                Priority = priority
            };

            var postResponse = await PostAsync<EditTaskRequest, bool>(baseUri + "/task" + "/edit", request);

            var response = ParseResponse<bool>(postResponse);

            return response;
        }

        public async Task<APIResponse<bool>> UpdateTaskCompletion(int id, bool done)
        {
            var request = new CompleteTaskRequest()
            {
                Id = id,
                Done = done
            };

            var postResponse = await PostAsync<CompleteTaskRequest, bool>(baseUri + "/task" + "/complete", request);

            var response = ParseResponse<bool>(postResponse);

            return response;
        }

        public async Task<APIResponse<TaskResponse>> GetTask(int taskId)
        {
            var parameters = new Dictionary<string, string>() { { "taskId", taskId.ToString() } };

            var getResponse = await GetAsync<TaskResponse>(baseUri + "/task" + "/get", parameters);

            var response = ParseResponse<TaskResponse>(getResponse);

            return response;
        }

        public async Task<APIResponse<PagedTaskResponse>> GetTasksPaged(int page, int pageSize, List<string> searchTerms)
        {
            var request = new GetTasksPagedRequest()
            {
                Page = page,
                PageSize = pageSize,
                SearchTerms = searchTerms
            };

            var postResponse = await PostAsync<GetTasksPagedRequest, PagedTaskResponse>(baseUri + "/task" + "/page", request);

            var response = ParseResponse<PagedTaskResponse>(postResponse);

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
