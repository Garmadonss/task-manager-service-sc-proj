namespace TaskManagerShared.Client.Request
{
    public class AddTaskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }             
        public DateTime Deadline { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
