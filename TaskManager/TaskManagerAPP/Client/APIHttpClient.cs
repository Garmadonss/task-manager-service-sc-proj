using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;


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

        public async Task<TResponse?> PostAsync(string requestUri, TRequest request)
        {           
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var httpResponse = httpClient.PostAsync(requestUri, content).Result;

            httpResponse.EnsureSuccessStatusCode();

            var response = (await httpResponse.Content.ReadFromJsonAsync<TResponse>());

            return response;
        }
    }
}
