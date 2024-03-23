using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaskManagerShared.Client;
using TaskManagerShared.Client.Response;

namespace TaskManagerAPP.ViewModel;

public partial class MainViewModel : ObservableObject
{
    IConnectivity connectivity;
    APIClient apiClient;
    SecureStorageBearerTokenProvider tokenProvider;

    [ObservableProperty]
    ObservableCollection<TaskViewModel> tasks = new ObservableCollection<TaskViewModel>();

    [ObservableProperty]
    TaskViewModel selectedTask;

    [ObservableProperty]
    string searchText = "";

    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    bool isLoading;

    [ObservableProperty]
    bool isRefreshing;    

    private int TotalTaskCount = 0;
    private int CurrentPage = -1;

    private const int PageSize = 10;

    public MainViewModel(IConnectivity connectivity, APIClient apiClient, SecureStorageBearerTokenProvider tokenProvider)
    {

        this.connectivity = connectivity;
        this.apiClient = apiClient;
        this.tokenProvider = tokenProvider;
    }

    public async Task GetNextTasksPage()
    {
        IsBusy = true;
        IsLoading = true;

        // TODO: Move to shared ViewModel component
        if (connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "OK");
            return;
        }

        try
        {
            var searchTerms = SearchText != null ? SearchText.Split(' ').Where( ts => ts.Length > 0).ToList() : new List<string>() ;
            var aPIResponse = await apiClient.GetTasksPaged(CurrentPage + 1, PageSize, searchTerms);

            if (aPIResponse.Sucessfull)
            {
                var pageResponse = aPIResponse.Response;

                TotalTaskCount = pageResponse.TotalCount;
                CurrentPage++;

                
                foreach (var task in pageResponse.Tasks)
                {
                    var newTask = new TaskViewModel(connectivity, apiClient)
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        DeadlineDate = task.Deadline,
                        DeadlineTime = task.Deadline.TimeOfDay,
                        IsSelected = false,
                        Done = task.Done,
                        Priority = task.Priority
                    };

                    Tasks.Add(newTask);
                }                
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
        finally
        {
            IsBusy = false;
            IsLoading = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task Refresh()
    {
        if (IsLoading) { return; }

        IsRefreshing = true;

        Tasks.Clear();
        TotalTaskCount = 0;
        CurrentPage = -1;

        await GetNextTasksPage();

        IsRefreshing = false;
    }

    [RelayCommand]    
    async Task LoadNextPage()
    {
        if (IsLoading) { return; }        

        if (Tasks.Count < TotalTaskCount)
        {
            await GetNextTasksPage();
        }
    }

    [RelayCommand]
    async Task SearchRefresh()
    {
        await Refresh();
    }

    [RelayCommand]
    async Task SearchTextChanged()
    {
        if (IsLoading) { return; } 

        if (SearchText.Length == 0) { 
            await Refresh();
        }
    }
    

    [RelayCommand]
    async Task AddNewTask()
    {
        await Shell.Current.GoToAsync(nameof(AddTaskPage));
    }

    [RelayCommand]
    public async Task MarkAsDoneAsync()
    {

        

    }

    [RelayCommand]
    async Task ClearTokenAndGoToLoginPage()
    {
        tokenProvider.ClearTokens();
        await Shell.Current.GoToAsync("..");
    }
}
