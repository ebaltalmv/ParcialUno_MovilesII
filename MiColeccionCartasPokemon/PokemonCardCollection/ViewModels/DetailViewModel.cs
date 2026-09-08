using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokemonCardCollection.Data;
using PokemonCardCollection.Models;

namespace PokemonCardCollection.ViewModels;

/// <summary>
/// ViewModel for the Detail page — shows full information about a selected card.
/// Enriches Rarity/Category/Description from the TCGdex API on demand.
/// </summary>
[QueryProperty(nameof(CardId), "cardId")]
public partial class DetailViewModel : ObservableObject
{
    private readonly PokemonCardRepository _repository;

    [ObservableProperty]
    private PokemonCard? _card;

    [ObservableProperty]
    private int _cardId;

    [ObservableProperty]
    private bool _isLoading;

    public DetailViewModel(PokemonCardRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Called automatically when CardId changes via query parameter.</summary>
    partial void OnCardIdChanged(int value)
    {
        _ = LoadCardAsync(value);
    }

    private async Task LoadCardAsync(int id)
    {
        var card = _repository.GetById(id);
        if (card is null) return;

        Card = card;

        if (card.ApiId is not null)
        {
            IsLoading = true;
            var detail = await _repository.GetCardDetailFromApiAsync(card.ApiId);
            IsLoading = false;

            if (detail is not null)
            {
                card.Rarity = detail.Rarity ?? card.Rarity;
                card.Category = detail.Types?.FirstOrDefault() ?? card.Category;
                card.Description = detail.Description ?? card.Description;

                // Force UI refresh
                Card = null;
                Card = card;
            }
        }
    }

    /// <summary>Toggles the favorite status of the current card.</summary>
    [RelayCommand]
    private void ToggleFavorite()
    {
        if (Card is null) return;

        _repository.ToggleFavorite(Card.Id);
        // Refresh the card object so the UI updates
        Card = _repository.GetById(Card.Id);
        OnPropertyChanged(nameof(Card));
    }

    /// <summary>Navigates to the Form page to edit the current card.</summary>
    [RelayCommand]
    private async Task GoToEditCard()
    {
        if (Card is null) return;

        await Shell.Current.GoToAsync($"FormPage?cardId={Card.Id}");
    }

    /// <summary>Deletes the current card after confirmation and navigates back.</summary>
    [RelayCommand]
    private async Task DeleteCard()
    {
        if (Card is null) return;

        bool confirm = await Shell.Current.DisplayAlertAsync(
            "Eliminar carta",
            $"¿Seguro que deseas eliminar {Card.Name}?",
            "Eliminar",
            "Cancelar");

        if (!confirm) return;

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
