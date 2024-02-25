using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPP.Client
{
    public class APIResponse<TResponseModel>
    {
        public bool Sucessfull { get; set; }
        public TResponseModel? Response { get; set; }
        public Dictionary<string, string>? ErrorDetails { get; set; }
    }
}
