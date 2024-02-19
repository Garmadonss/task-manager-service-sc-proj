using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPP.Client.NewFolder
{
    public class AccessTokenResponse
    {
        public string? TokenType { get; set; }
        public string? AccessToken { get; set; }
        public int? ExpiresIn { get; set; }
        public string? RefreshToken { get; set; }
    }
}
