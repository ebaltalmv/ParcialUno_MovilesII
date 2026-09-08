using PokemonCardCollection.ViewModels;

namespace PokemonCardCollection.Views;

public partial class FavoritesPage : ContentPage
{
    public FavoritesPage(FavoritesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Recargar las favoritas cada vez que la pantalla aparece
        if (BindingContext is FavoritesViewModel vm)
            await vm.LoadFavoritesCommand.ExecuteAsync(null);
    }
}
