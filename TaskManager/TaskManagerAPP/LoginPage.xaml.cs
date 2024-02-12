using TaskManagerAPP.ViewModel;

namespace TaskManagerAPP;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();        
        BindingContext = vm;
    }
}