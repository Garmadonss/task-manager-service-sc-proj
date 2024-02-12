using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManagerShared
{
    public class TaskManagerUser : IdentityUser { } 

    public class TaskManagerDBContext : IdentityDbContext<TaskManagerUser>
    {
        public TaskManagerDBContext(DbContextOptions<TaskManagerDBContext> options) : base(options) { }
    }
}
