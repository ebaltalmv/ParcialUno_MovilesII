using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokemonCardCollection.Data;
using PokemonCardCollection.Models;
using PokemonCardCollection.Services;

namespace PokemonCardCollection.ViewModels;

/// <summary>
/// ViewModel for the List page.
/// </summary>
public partial class ListViewModel : ObservableObject
{
    private const int PageSize = 30;
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

    /// <summary>Deletes a card after user confirmation.</summary>
    [RelayCommand]
    private async Task DeleteCard(PokemonCard card)
    {
        if (card is null) return;

        bool confirm = await Shell.Current.DisplayAlertAsync(
            "Eliminar carta",
            $"¿Seguro que deseas eliminar {card.Name}?",
            "Eliminar",
            "Cancelar");

        if (!confirm) return;

        _repository.Delete(card.Id);
        Cards = new ObservableCollection<PokemonCard>(_repository.GetAll());
    }
}
