using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokemonCardCollection.Data;
using PokemonCardCollection.Models;

namespace PokemonCardCollection.ViewModels;

/// <summary>
/// ViewModel for the List page — displays all cards in the collection.
/// </summary>
public partial class ListViewModel : ObservableObject
{
    private readonly PokemonCardRepository _repository;

    [ObservableProperty]
    private ObservableCollection<PokemonCard> _cards = new();

    public ListViewModel(PokemonCardRepository repository)
    {
        _repository = repository;
        LoadCards();
    }

    /// <summary>Reloads the full card list from the repository.</summary>
    [RelayCommand]
    private void LoadCards()
    {
        Cards = new ObservableCollection<PokemonCard>(_repository.GetAll());
    }

    /// <summary>Navigates to the Detail page for the selected card.</summary>
    [RelayCommand]
    private async Task GoToDetail(PokemonCard card)
    {
        if (card is null) return;

        await Shell.Current.GoToAsync($"DetailPage?cardId={card.Id}");
    }

    /// <summary>Navigates to the Form page to add a new card.</summary>
    [RelayCommand]
    private async Task GoToAddCard()
    {
        await Shell.Current.GoToAsync("FormPage");
    }
}
