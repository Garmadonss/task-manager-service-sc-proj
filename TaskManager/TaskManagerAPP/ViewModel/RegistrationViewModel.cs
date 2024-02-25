using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPP.Client;

namespace TaskManagerAPP.ViewModel
{
    public partial class RegistrationViewModel : ObservableObject
    {
        [ObservableProperty]
        string email;

        [ObservableProperty]
        string password;

        [ObservableProperty]
        string passwordRepeated;

        IConnectivity connectivity;

        APIClient apiHttpClient;

        public RegistrationViewModel(IConnectivity connectivity)
        {
            this.email = string.Empty;
            this.password = string.Empty;
            this.passwordRepeated = string.Empty;
            this.connectivity = connectivity;
            this.apiHttpClient = new APIClient(); // TODO: add configs and etc
        }

        [RelayCommand]
        private async Task Register(object sender)
        {
            // TODO: Move to shared ViewModel component
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "OK");
                return;
            }

            if (!await Validate()) return;

            try
            {               
                var aPIResponse = await apiHttpClient.RegisterAsync(Email, Password);

                if (aPIResponse.Sucessfull)
                {
                    await Shell.Current.DisplayAlert($"You have been successfully registered", "", "Back to login page");
                    await Shell.Current.GoToAsync(nameof(LoginPage));
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
        }

        private async Task<bool> Validate() { 

            if (Password != PasswordRepeated)
            {
                await Shell.Current.DisplayAlert($"Passwords do not match", "", "OK");
                return false;
            }

            return true; 
        }
    }
}
