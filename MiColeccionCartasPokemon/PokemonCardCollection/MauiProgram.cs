using Microsoft.Extensions.Logging;
using PokemonCardCollection.Data;
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

        // Data — singleton so all pages share the same in-memory data
        builder.Services.AddSingleton<PokemonCardRepository>();

        // ViewModels — transient so each navigation gets a fresh instance
        builder.Services.AddTransient<ListViewModel>();
        builder.Services.AddTransient<DetailViewModel>();
        builder.Services.AddTransient<FavoritesViewModel>();
        builder.Services.AddTransient<FormViewModel>();

        // Views (Pages) — transient
        builder.Services.AddTransient<ListPage>();
        builder.Services.AddTransient<DetailPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<FormPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
