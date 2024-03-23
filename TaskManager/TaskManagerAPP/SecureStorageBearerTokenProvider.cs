using TaskManagerShared.Client;

namespace TaskManagerAPP
{
    public class SecureStorageBearerTokenProvider : IBearerTokenProvider
    {
        public void StoreAccessToken(string accessToken)
        {
            Task.Run(async () => await SecureStorage.Default.SetAsync("access_token", accessToken));
        }

        public void StoreRefreshToken(string accessToken)
        {
            Task.Run(async () => await SecureStorage.Default.SetAsync("refresh_token", accessToken));
        }

        public string? GetAccessToken()
        {
            var accessToken = Task.Run(async () => await SecureStorage.Default.GetAsync("access_token")).Result;
            return accessToken;
        }

        public string? GetRefreshToken()
        {
            var refreshToken = Task.Run(async () => await SecureStorage.Default.GetAsync("refresh_token")).Result;
            return refreshToken;
        }

        public void ClearTokens()
        {
            SecureStorage.Default.RemoveAll();
        }
    }
}
