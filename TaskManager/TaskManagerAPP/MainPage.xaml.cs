using TaskManagerAPP.ViewModel;

namespace TaskManagerAPP;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
