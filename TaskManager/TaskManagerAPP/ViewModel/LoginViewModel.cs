using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManagerAPP.Client;

namespace TaskManagerAPP.ViewModel
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        string email;
        
        [ObservableProperty]
        string password;

        IConnectivity connectivity;       

        APIClient apiHttpClient;

        public LoginViewModel(IConnectivity connectivity)
        {
            this.email = string.Empty;
            this.password = string.Empty;
            this.connectivity = connectivity;            
            this.apiHttpClient = new APIClient(); // TODO: add configs and etc
        }

        [RelayCommand]
        private async Task Login(object sender)
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "OK");
                return;
            }

            try
            {
                var accessTokenResponse = await apiHttpClient.LoginAsync(Email, Password);

                await Shell.Current.DisplayAlert($"Acess Token Response {accessTokenResponse.AccessToken} {Email} {Password}", "", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Oooops", "Something went wrong", "OK");


                var a = ex.Message;
            }
        }

        [RelayCommand]
        private async Task ToRegistrationPage(object sender)
        {
            await Shell.Current.GoToAsync(nameof(RegistrationPage));

        }
    }
}
