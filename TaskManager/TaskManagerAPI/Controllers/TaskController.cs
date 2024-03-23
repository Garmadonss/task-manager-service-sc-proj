using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TaskManagerShared;
using TaskManagerShared.Client.Request;
using TaskManagerShared.Client.Response;
using Task = TaskManagerShared.Task;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly TaskManagerDBContext _context;        

        public TaskController(ILogger<TaskController> logger, TaskManagerDBContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "Get")]
        public TaskResponse Get(int taskId)
        {

            var userId = GetUserId();

            var task = _context.Tasks.FirstOrDefault(t => t.UserId == userId && t.Id == taskId);

            var response = new TaskResponse()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Done = task.Done,
                Deadline = task.Deadline,
                Priority = task.Priority,
            };

            return response;
        }

        [Authorize]
        [HttpPost(Name = "Page")]
        public PagedTaskResponse Page(GetTasksPagedRequest request)
        {
            var userId = GetUserId();

            bool containsSearchTerms = request != null && request.SearchTerms.Count > 0 && request.SearchTerms.Any(s => !s.IsNullOrEmpty());

            var totalCount = _context.Tasks
                // Belongs to User and where Title or Description contains search terms
                .Where(t => (t.UserId == userId)
                    // If containst search terms
                    && (containsSearchTerms ? request.SearchTerms.Any(st => t.Title.Contains(st) || request.SearchTerms.Any(st => t.Description.Contains(st))) : true))
                .Count();            

            var pageTasks = _context.Tasks
                // Belongs to User and where Title or Description contains search terms
                .Where(t => (t.UserId == userId) 
                    // If containst search terms
                    && (containsSearchTerms ? request.SearchTerms.Any(st => t.Title.Contains(st) || request.SearchTerms.Any(st => t.Description.Contains(st))) : true))
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var response = new PagedTaskResponse()
            {
                TotalCount = totalCount,
                Tasks = pageTasks.ConvertAll(t =>
                    new TaskResponse()
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        Done = t.Done,
                        Deadline = t.Deadline,
                        Priority = t.Priority,
                    }
                )
            };

            return response;
        }

        [Authorize]
        [HttpPost(Name = "Add")]
        public void Add(AddTaskRequest request)
        {
            var userId = GetUserId();

            _context.Tasks
                .Add(
                    new Task()
                    {
                        UserId = userId,
                        Title = request.Title,
                        Description = request.Description,
                        Done = false,
                        DateCreated = DateTime.UtcNow,
                        Deadline = request.Deadline,
                        Priority = request.Priority
                    }
                    );
            _context.SaveChanges();
        }

        [Authorize]
        [HttpPost(Name = "Edit")]
        public void Edit(EditTaskRequest request)
        {            
            var userId = GetUserId();

            var taskEntityToChange = _context.Tasks
                .FirstOrDefault( t => t.UserId == userId && t.Id == request.Id);

            taskEntityToChange.Title = request.Title;
            taskEntityToChange.Description = request.Description;
            taskEntityToChange.Done = request.Done;
            taskEntityToChange.Deadline = request.Deadline;
            taskEntityToChange.Priority = request.Priority;

            _context.SaveChanges();
        }

        [Authorize]
        [HttpPost(Name = "Complete")]
        public void Complete(CompleteTaskRequest request)
        {
            var userId = GetUserId();

            var taskEntityToChange = _context.Tasks
                .FirstOrDefault(t => t.UserId == userId && t.Id == request.Id);
            
            taskEntityToChange.Done = request.Done;            

            _context.SaveChanges();
        }

        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claim.Value;
        }
    }
}
