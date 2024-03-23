using TaskManagerAPP.ViewModel;
using TaskManagerShared.Client;

namespace TaskManagerAPP;

public partial class AddTaskPage : ContentPage
{
	public AddTaskPage(IConnectivity connectivity, APIClient apiClient)
	{
		InitializeComponent();
		BindingContext = new TaskViewModel(connectivity, apiClient);
    }    
}