using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PokemonCardCollection.Data;
using PokemonCardCollection.Services;
using PokemonCardCollection.ViewModels;
using PokemonCardCollection.Views;

namespace PokemonCardCollection;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ----- Dependency Injection -----

        // Configuración de la URL base del backend OData local:
        // - En emulador Android: '10.0.2.2' es el alias de loopback para acceder al host de desarrollo.
        // - En Windows / MacCatalyst / iOS Simulator: 'localhost' apunta directamente a la máquina local.
        var backendBaseUrl = DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5123/"
            : "http://localhost:5123/";

        // Services
        builder.Services.AddHttpClient<IPokemonCardService, PokemonCardService>(client =>
        {
            client.BaseAddress = new Uri(backendBaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(15);
        });

        builder.Services.AddSingleton<PokemonApiService>();

        // Data — singleton so all pages share the same in-memory data
        builder.Services.AddSingleton<PokemonCardRepository>();

        // ViewModels — transient so each navigation gets a fresh instance
        builder.Services.AddTransient<ListViewModel>();
        builder.Services.AddTransient<DetailViewModel>();
        builder.Services.AddSingleton<FavoritesViewModel>();
        builder.Services.AddTransient<FormViewModel>();

        // Views (Pages) — transient
        builder.Services.AddTransient<ListPage>();
        builder.Services.AddTransient<DetailPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<FormPage>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif
        builder.Services.AddMauiBlazorWebView();

        return builder.Build();
    }
}
