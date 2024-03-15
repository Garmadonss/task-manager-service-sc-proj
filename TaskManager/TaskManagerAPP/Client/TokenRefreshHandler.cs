using System.Net.Http.Headers;

namespace TaskManagerAPP.Client
{
    public class TokenRefreshHandler : DelegatingHandler
    {
        private readonly APIClient apiClient;

        public TokenRefreshHandler(APIClient apiClient)
        {
            this.apiClient = apiClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var accessToken = SecureStorageHelper.GetAccessToken();
            var refreshToken = SecureStorageHelper.GetRefreshToken();

            if (accessToken != null)
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var responseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized && refreshToken != null)
            {
                var refreshResult = await apiClient.RefreshTokenAsync(refreshToken);

                if (!refreshResult.Sucessfull)
                {
                    var refreshResponse = refreshResult.Response;

                    request.Headers.Authorization =
                            new AuthenticationHeaderValue("Bearer", refreshResponse?.AccessToken);
//
                    responseMessage = await base.SendAsync(request, cancellationToken);
                }
            }

            return responseMessage;
        }
    }
}
