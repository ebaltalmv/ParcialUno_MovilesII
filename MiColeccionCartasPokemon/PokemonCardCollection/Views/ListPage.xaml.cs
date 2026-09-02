using PokemonCardCollection.ViewModels;

namespace PokemonCardCollection.Views;

public partial class ListPage : ContentPage
{
    public ListPage(ListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh the list every time the page appears (e.g. after add/edit/delete)
        if (BindingContext is ListViewModel vm)
            vm.LoadCardsCommand.Execute(null);
    }
}
