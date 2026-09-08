using PokemonCardCollection.ViewModels;

namespace PokemonCardCollection.Views;

public partial class FormPage : ContentPage
{
    public FormPage(FormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is FormViewModel vm)
            await vm.InitializePickersCommand.ExecuteAsync(null);
    }
}
