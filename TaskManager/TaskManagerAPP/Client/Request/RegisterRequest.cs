using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPP.Client.Requests
{
    internal class RegisterRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
