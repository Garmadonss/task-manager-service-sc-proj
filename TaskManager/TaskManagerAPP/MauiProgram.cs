using TaskManagerAPP.ViewModel;
using TaskManagerApp.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaskManagerShared.Client;
using MauiIcons.Fluent;
using CommunityToolkit.Maui;
using System.Reflection;


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

            builder.AddAppSettings();

            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

            var tokenProvider = new SecureStorageBearerTokenProvider();

            builder.Services.AddSingleton(tokenProvider);

            String apiBaseUrl = builder.Configuration.GetValue<string>("API_BASE_URL");

            builder.Services.AddSingleton(new APIClient(tokenProvider, HttpsClientHandlerService.GetPlatformMessageHandler(), apiBaseUrl));


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

        public static void AddAppSettings(this MauiAppBuilder builder)
        {
            using Stream stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("TaskManagerAPP.appsettings.json");

            if(stream != null)
            {
                IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                builder.Configuration.AddConfiguration(configurationRoot);
            }

        }
    }
}
