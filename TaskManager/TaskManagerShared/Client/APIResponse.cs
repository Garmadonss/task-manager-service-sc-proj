namespace TaskManagerShared.Client
{
    public class APIResponse<TResponseModel>
    {
        public bool Sucessfull { get; set; }
        public TResponseModel? Response { get; set; }
        public Dictionary<string, string>? ErrorDetails { get; set; }
    }
}
