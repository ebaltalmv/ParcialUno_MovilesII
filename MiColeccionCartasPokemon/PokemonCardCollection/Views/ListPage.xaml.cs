using PokemonCardCollection.ViewModels;

namespace PokemonCardCollection.Views;

public partial class ListPage : ContentPage
{
    public ListPage(ListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}
