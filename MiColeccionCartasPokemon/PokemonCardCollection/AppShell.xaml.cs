using PokemonCardCollection.Views;

namespace PokemonCardCollection;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for pages that are navigated to programmatically (push navigation)
        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        Routing.RegisterRoute(nameof(FormPage), typeof(FormPage));
    }
}
