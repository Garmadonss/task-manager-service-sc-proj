using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerShared
{
    [PrimaryKey(nameof(Id))]
    public class TaskManagerUser : IdentityUser 
    {
        public ICollection<Task> Tasks { get; }
    }

    [PrimaryKey(nameof(Id))]
    public class Task
    {
        [Required]
        public TaskManagerUser User { get; set; }
        [Required]
        public required string UserId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required bool Done { get; set; }
        [Required]
        public required DateTime DateCreated { get; set; }
        [Required]
        public required DateTime Deadline { get; set; }
        [Required]
        public required TaskPriority Priority { get; set; }
    }

    public class TaskManagerDBContext : IdentityDbContext<TaskManagerUser>
    {
        public TaskManagerDBContext(DbContextOptions<TaskManagerDBContext> options) : base(options) { }

        public TaskManagerDBContext() : base() { }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaskManagerUser>()
                .HasMany(t => t.Tasks)
                .WithOne(u => u.User)
                .HasForeignKey(t => t.UserId)
                .HasPrincipalKey(t => t.Id);
        }
    }    
}
