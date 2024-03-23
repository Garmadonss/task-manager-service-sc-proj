using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerShared.Client.Request
{
    public class GetTasksPagedRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<string> SearchTerms { get; set; }
    }
}
