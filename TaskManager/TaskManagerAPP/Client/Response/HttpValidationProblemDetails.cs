using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPP.Client.Response
{
    public class HttpValidationProblemDetails
    {
        [JsonProperty("type")]
        public string? Type {  get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("detail")]
        public string? Detail { get; set; }

        [JsonProperty("instance")]
        public string? Instance { get; set; }

        [JsonProperty("errors")]
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
