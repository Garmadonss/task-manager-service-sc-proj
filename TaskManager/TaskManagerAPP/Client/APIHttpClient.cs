using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using TaskManagerAPP.Client.Response;


namespace TaskManagerAPP.Client
{
    internal class APIHttpClient<TRequest, TResponse>
    {
        private readonly HttpClient httpClient = CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            HttpClient httpClient;
#if DEBUG
            HttpsClientHandlerService handler = new HttpsClientHandlerService();
            httpClient = new HttpClient(handler.GetPlatformMessageHandler());
#else
            httpClient = new HttpClient();
#endif
            return httpClient;
        }

        public async Task<APIHttpClientResponse<TResponse>> PostAsync(string requestUri, TRequest request)
        {           
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var httpResponse = httpClient.PostAsync(requestUri, content).Result;

            string responseJson =  await httpResponse.Content.ReadAsStringAsync();

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
    }
}
