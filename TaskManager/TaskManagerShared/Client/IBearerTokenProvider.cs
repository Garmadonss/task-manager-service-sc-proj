namespace TaskManagerShared.Client
{
    public interface IBearerTokenProvider
    {
        public void StoreAccessToken(string accessToken);
        public void StoreRefreshToken(string refreshToken);
        public string? GetAccessToken();
        public string? GetRefreshToken();
        public void ClearTokens();
    }
}
