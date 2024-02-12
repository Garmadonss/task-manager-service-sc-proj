using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPP.Clients
{
    class APIClient
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static string login(string username, string password)
        {
            var values = new Dictionary<string, string>
              {
                  { "username", "password" },
                  { "password", "world" }
              };

            var content = new FormUrlEncodedContent(values);

            //var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);

            //var responseString = await response.Content.ReadAsStringAsync();
            return null;
        }

    }
}
