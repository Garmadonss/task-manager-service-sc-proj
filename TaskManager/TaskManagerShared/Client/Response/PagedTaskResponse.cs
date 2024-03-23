using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerShared.Client.Response
{
    public class PagedTaskResponse
    {
        public int TotalCount { get; set; }

        public List<TaskResponse> Tasks { get; set; }
    }
}
