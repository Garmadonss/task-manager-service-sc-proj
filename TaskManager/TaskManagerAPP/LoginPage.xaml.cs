using TaskManagerAPP.ViewModel;

namespace TaskManagerAPP;

public partial class LoginPage : ContentPage
{
    LoginViewModel vm;

    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        NavigationPage.SetHasBackButton(this, false);
        this.vm = vm;
        BindingContext = vm;
    }

    override protected void OnAppearing()
    {
        if (this.vm.tokenProvider.GetAccessToken() != null)
        {
            var aa = vm.ToMainPageCommand.CanExecute(null);
            //vm.ToMainPageCommand.Execute(null);
        }
    }
}