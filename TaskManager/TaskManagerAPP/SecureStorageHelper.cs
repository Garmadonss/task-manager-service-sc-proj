using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPP
{
    public static class SecureStorageHelper
    {
        public static string? GetAccessToken()
        {
            var accessToken = Task.Run(async () => await SecureStorage.Default.GetAsync("access_token")).Result;
            return accessToken;
        }

        public static string? GetRefreshToken()
        {
            var refreshToken = Task.Run(async () => await SecureStorage.Default.GetAsync("refresh_token")).Result;
            return refreshToken;
        }
    }
}
