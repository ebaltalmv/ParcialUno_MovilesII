using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokemonCardCollection.Data;
using PokemonCardCollection.Models;

namespace PokemonCardCollection.ViewModels;

/// <summary>
/// ViewModel for the List page.
/// </summary>
public partial class ListViewModel : ObservableObject
{
    private readonly PokemonCardRepository _repository;

    public ObservableCollection<PokemonCard> Cards => _repository.Cards;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public ListViewModel(PokemonCardRepository repository)
    {
        _repository = repository;
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;
        OnPropertyChanged(nameof(HasError));

        var error = await _repository.InitializeAsync();
        
        if (error != null)
        {
            ErrorMessage = error;
            OnPropertyChanged(nameof(HasError));
        }
        
        IsLoading = false;
    }

    [RelayCommand]
    private async Task RetryLoad()
    {
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task GoToDetail(PokemonCard card)
    {
        if (card is null) return;

        await Shell.Current.GoToAsync($"DetailPage?cardId={card.Id}");
    }

    [RelayCommand]
    private async Task GoToAddCard()
    {
        await Shell.Current.GoToAsync("FormPage");
    }
}
