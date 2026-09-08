using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokemonCardCollection.Data;
using PokemonCardCollection.Models;

namespace PokemonCardCollection.ViewModels;

/// <summary>
/// ViewModel for the Favorites / Wishlist page — shows only cards marked as favorite.
/// </summary>
public partial class FavoritesViewModel : ObservableObject
{
    private readonly PokemonCardRepository _repository;

    [ObservableProperty]
    private ObservableCollection<PokemonCard> _favoriteCards = new();

    public FavoritesViewModel(PokemonCardRepository repository)
    {
        _repository = repository;
        _repository.Cards.CollectionChanged += (s, e) => LoadFavorites();
        LoadFavorites();
    }

    [RelayCommand]
    public void LoadFavorites()
    {
        FavoriteCards = new ObservableCollection<PokemonCard>(_repository.Cards.Where(c => c.IsFavorite));
    }

    // Navega a la pantalla de detalle de la carta seleccionada
    [RelayCommand]
    private async Task GoToDetail(PokemonCard card)
    {
        if (card is null) return;

        await Shell.Current.GoToAsync($"DetailPage?cardId={card.Id}");
    }
}
