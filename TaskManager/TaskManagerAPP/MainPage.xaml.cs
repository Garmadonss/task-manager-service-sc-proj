using MauiIcons.Core;
using TaskManagerAPP.ViewModel;
using TaskManagerShared.Client.Response;

namespace TaskManagerAPP;

public partial class MainPage : ContentPage
{

    private MainViewModel vm;

    public MainPage(MainViewModel vm)
    {
        _ = new MauiIcon();
        this.vm = vm;
        BindingContext = vm;
        InitializeComponent();
            
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await vm.GetNextTasksPage();
    }

    protected override bool OnBackButtonPressed()
    {
        vm.ClearTokenAndGoToLoginPageCommand.ExecuteAsync(null);
        return true;
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedTask = (e.CurrentSelection.FirstOrDefault() as TaskViewModel);

        Shell.Current.GoToAsync(nameof(EditTaskPage), new Dictionary<string, Object>() { { "task_id", selectedTask.Id } });
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue == string.Empty)
        {
            vm.SearchTextChangedCommand.Execute(null);
        }               
    }    

    //private void IsDone_CheckedChanged(object sender, CheckedChangedEventArgs e)
    //{
    //    bool isDone = e.Value;e.
    //    vm.MarkAsDoneCommand.ExecuteAsync(null);
    //}
}
