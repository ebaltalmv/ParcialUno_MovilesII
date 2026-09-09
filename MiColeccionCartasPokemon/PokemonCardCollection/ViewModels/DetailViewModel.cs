using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokemonCardCollection.Data;
using PokemonCardCollection.Models;

namespace PokemonCardCollection.ViewModels;

/// <summary>
/// ViewModel for the Detail page.
/// </summary>
[QueryProperty(nameof(CardId), "cardId")]
public partial class DetailViewModel : ObservableObject
{
    private readonly PokemonCardRepository _repository;

    [ObservableProperty]
    private PokemonCard? _card;

    [ObservableProperty]
    private string _cardId = string.Empty;

    public DetailViewModel(PokemonCardRepository repository)
    {
        _repository = repository;
    }

    partial void OnCardIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            Card = _repository.GetById(value);
        }
    }

    [RelayCommand]
    private void ToggleFavorite()
    {
        if (Card is null) return;

        _repository.ToggleFavorite(Card.Id);
        Card = _repository.GetById(Card.Id);
        OnPropertyChanged(nameof(Card));
    }

    [RelayCommand]
    private async Task GoToEditCard()
    {
        if (Card is null) return;

        await Shell.Current.GoToAsync($"FormPage?cardId={Card.Id}");
    }

    [RelayCommand]
    private async Task EliminarArticulo()
    {
        if (Card is null) return;

        bool answer = await Shell.Current.DisplayAlertAsync("Confirm Delete", $"Are you sure you want to delete {Card.Name}?", "Yes", "No");
        
        if (answer)
        {
            _repository.Delete(Card.Id);
            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
