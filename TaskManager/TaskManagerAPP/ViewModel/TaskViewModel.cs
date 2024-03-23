using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManagerShared;
using TaskManagerShared.Client;

namespace TaskManagerAPP.ViewModel
{
    public partial class TaskViewModel : ObservableObject
    {
        [ObservableProperty]
        public int id;

        [ObservableProperty]
        public string title;

        [ObservableProperty]
        public string description;

        [ObservableProperty]
        public bool done;

        [ObservableProperty]
        public bool isSelected;

        [ObservableProperty]
        public DateTime dateCreated;

        [ObservableProperty]
        public DateTime deadlineDate;

        [ObservableProperty]
        public TimeSpan deadlineTime;

        [ObservableProperty]
        public TaskPriority priority;

        [ObservableProperty]
        public bool busy;
        
        public DateTime deadline => deadlineDate.Date + deadlineTime;
        
        public List<TaskPriority> availablePriorities => new List<TaskPriority> { TaskPriority.Low, TaskPriority.Normal, TaskPriority.High };

        [ObservableProperty]
        IConnectivity connectivity;

        [ObservableProperty]
        APIClient apiClient;

        public TaskViewModel(IConnectivity connectivity, APIClient apiClient)
        {
            this.connectivity = connectivity;
            this.apiClient = apiClient;
            this.deadlineDate = DateTime.UtcNow;
        }        

        [RelayCommand]
        private async System.Threading.Tasks.Task AddTaskAsync()
        {
            // TODO: Move to shared ViewModel component
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "OK");
                return;
            }

            try
            { 
                var aPIResponse = await apiClient.AddTaskAsync(Title, Description, DeadlineDate.Date + DeadlineTime, Priority);

                if (aPIResponse.Sucessfull)
                {
                    await Shell.Current.DisplayAlert($"Task has been successfully added", "", "Back to main page");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert($"{aPIResponse?.ErrorDetails?.First().Value}", "", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Oooops", "Something went wrong", "OK");

                var a = ex.Message;
            }
        }

        [RelayCommand]
        private async System.Threading.Tasks.Task EditTaskAsync()
        {
            // TODO: Move to shared ViewModel component
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "OK");
                return;
            }

            try
            {
                var aPIResponse = await apiClient.EditTaskAsync(Id, Title, Description, Done, DeadlineDate.Date + DeadlineTime, Priority);

                if (aPIResponse.Sucessfull)
                {
                    await Shell.Current.DisplayAlert($"Task has been successfully edited", "", "Back to task");                    
                }
                else
                {
                    await Shell.Current.DisplayAlert($"{aPIResponse?.ErrorDetails?.First().Value}", "", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Oooops", "Something went wrong", "OK");

                var a = ex.Message;
            }
        }

        [RelayCommand]
        private async System.Threading.Tasks.Task Refresh()
        {
            // TODO: Move to shared ViewModel component
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "OK");
                return;
            }

            try
            {
                var aPIResponse = await apiClient.GetTask(Id);

                if (!aPIResponse.Sucessfull)
                {                
                    await Shell.Current.DisplayAlert($"{aPIResponse?.ErrorDetails?.First().Value}", "", "OK");
                }

                var taskResponse = aPIResponse.Response;

                Id = taskResponse.Id;
                Title = taskResponse.Title;
                Description = taskResponse.Description;
                Done = taskResponse.Done;
                DeadlineDate = taskResponse.Deadline;
                DeadlineTime = taskResponse.Deadline.TimeOfDay;
                Priority = taskResponse.Priority;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Oooops", "Something went wrong", "OK");

                var a = ex.Message;
            }
        }

        [RelayCommand]
        async System.Threading.Tasks.Task UpdateDoneAsync(bool state)
        {
            // TODO: Move to shared ViewModel component
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "OK");
                return;
            }

            try
            {
                var aPIResponse = await apiClient.UpdateTaskCompletion(Id, Done);

                if (aPIResponse.Sucessfull)
                {
                    await Shell.Current.DisplayAlert($"Task completion has been sucessfully updated", "", "Back to tasks");
                }
                else
                {
                    await Shell.Current.DisplayAlert($"{aPIResponse?.ErrorDetails?.First().Value}", "", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Oooops", "Something went wrong", "OK");

                var a = ex.Message;
            }
        }

        [RelayCommand]
        private async System.Threading.Tasks.Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }      
}
