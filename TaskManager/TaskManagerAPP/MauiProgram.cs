using TaskManagerAPP.ViewModel;
using TaskManagerApp.Client;
using Microsoft.Extensions.Logging;
using TaskManagerShared.Client;
using MauiIcons.Fluent;
using CommunityToolkit.Maui;


namespace TaskManagerAPP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

            var tokenProvider = new SecureStorageBearerTokenProvider();

            builder.Services.AddSingleton(tokenProvider);
            builder.Services.AddSingleton(new APIClient(tokenProvider, HttpsClientHandlerService.GetPlatformMessageHandler()));


            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginViewModel>();

            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationViewModel>();

            builder.Services.AddTransient<AddTaskPage>();
            builder.Services.AddTransient<EditTaskPage>();

            builder.UseMauiApp<App>().UseFluentMauiIcons();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
