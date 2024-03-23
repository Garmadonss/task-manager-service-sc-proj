using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerShared.Client.Request
{
    public class EditTaskRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime Deadline { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
