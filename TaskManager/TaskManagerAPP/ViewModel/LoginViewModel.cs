using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManagerShared.Client;


namespace TaskManagerAPP.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        string email;
        
        [ObservableProperty]
        string password;

        IConnectivity connectivity;       

        APIClient apiClient;

        public SecureStorageBearerTokenProvider tokenProvider;

        public LoginViewModel(IConnectivity connectivity, APIClient apiClient, SecureStorageBearerTokenProvider tokenProvider)
        {
            this.email = string.Empty;
            this.password = string.Empty;
            this.connectivity = connectivity;
            this.apiClient = apiClient;
            this.tokenProvider = tokenProvider;            
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
                var aPIResponse = await apiClient.LoginAsync(Email, Password);

                var aa = tokenProvider.GetAccessToken();
                var bb = tokenProvider.GetRefreshToken();


                if (aPIResponse.Sucessfull)
                {
                    tokenProvider.StoreAccessToken(aPIResponse.Response.AccessToken);
                    tokenProvider.StoreRefreshToken(aPIResponse.Response.RefreshToken);

                    await Shell.Current.GoToAsync(nameof(MainPage));
                }
                else
                {
                    await Shell.Current.DisplayAlert($"Failed to login", "Please try again", "OK");
                }
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

        [RelayCommand]
        private async Task ToMainPage(object sender)
        {
            await Shell.Current.GoToAsync(nameof(MainPage));
        }
    }
}
