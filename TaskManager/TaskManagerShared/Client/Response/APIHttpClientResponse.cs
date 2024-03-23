using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerShared.Client.Response
{
    public class APIHttpClientResponse<TResponse>
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public TResponse? Response { get; set; }
        public HttpValidationProblemDetails? ProblemDetails { get; set; }
    }
}
