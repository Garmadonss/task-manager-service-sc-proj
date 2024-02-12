using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Networking;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TaskManagerAPP.ViewModel
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        string userName;
        
        [ObservableProperty]
        string password;

        IConnectivity connectivity;

        public LoginViewModel(IConnectivity connectivity)
        {
            this.userName = string.Empty;
            this.password = string.Empty;
            this.connectivity = connectivity;
        }

        [RelayCommand]
        private async Task Login(object sender)
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "OK");
                return;
            }


            App.Current.MainPage.DisplayAlert($"Hi {UserName} {Password}", "", "OK");
        }

    }
}
