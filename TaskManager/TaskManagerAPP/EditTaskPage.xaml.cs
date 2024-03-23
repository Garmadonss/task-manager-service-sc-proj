using TaskManagerAPP.ViewModel;
using TaskManagerShared.Client;

namespace TaskManagerAPP;

public partial class EditTaskPage : ContentPage, IQueryAttributable
{
	public EditTaskPage(IConnectivity connectivity, APIClient apiClient)
    {
        InitializeComponent();
        BindingContext = new TaskViewModel(connectivity, apiClient);
    }

    async void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> parameters)
    {
        var taskId = (int)parameters["task_id"];
        TaskViewModel taskViewModel = (TaskViewModel)BindingContext;
        taskViewModel.Id = taskId;
        taskViewModel.RefreshCommand.Execute(null);     
    }
}