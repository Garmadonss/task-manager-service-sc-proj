using TaskManagerAPP.ViewModel;

namespace TaskManagerAPP;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage(RegistrationViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}