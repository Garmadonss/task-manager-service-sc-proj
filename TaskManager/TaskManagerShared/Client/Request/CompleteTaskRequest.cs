using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerShared.Client.Request
{
    public class CompleteTaskRequest
    {
        public int Id { get; set; }
        public bool Done {  get; set; }
    }
}
