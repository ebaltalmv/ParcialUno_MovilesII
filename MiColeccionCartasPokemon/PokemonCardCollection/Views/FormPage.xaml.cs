using PokemonCardCollection.ViewModels;

namespace PokemonCardCollection.Views;

public partial class FormPage : ContentPage
{
    public FormPage(FormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
