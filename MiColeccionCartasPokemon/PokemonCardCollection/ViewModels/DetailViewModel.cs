using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokemonCardCollection.Data;
using PokemonCardCollection.Models;

namespace PokemonCardCollection.ViewModels;

/// <summary>
/// ViewModel for the Detail page — shows full information about a selected card.
/// </summary>
[QueryProperty(nameof(CardId), "cardId")]
public partial class DetailViewModel : ObservableObject
{
    private readonly PokemonCardRepository _repository;

    [ObservableProperty]
    private PokemonCard? _card;

    [ObservableProperty]
    private int _cardId;

    public DetailViewModel(PokemonCardRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Called automatically when CardId changes via query parameter.</summary>
    partial void OnCardIdChanged(int value)
    {
        Card = _repository.GetById(value);
    }

    /// <summary>Toggles the favorite status of the current card.</summary>
    [RelayCommand]
    private void ToggleFavorite()
    {
        if (Card is null) return;

        _repository.ToggleFavorite(Card.Id);
        // Refresh the card object so the UI updates
        Card = _repository.GetById(Card.Id);
    }

    /// <summary>Navigates to the Form page to edit the current card.</summary>
    [RelayCommand]
    private async Task GoToEditCard()
    {
        if (Card is null) return;

        await Shell.Current.GoToAsync($"FormPage?cardId={Card.Id}");
    }

    /// <summary>Deletes the current card and navigates back.</summary>
    [RelayCommand]
    private async Task DeleteCard()
    {
        if (Card is null) return;

        _repository.Delete(Card.Id);
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>Navigates back to the previous page.</summary>
    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
