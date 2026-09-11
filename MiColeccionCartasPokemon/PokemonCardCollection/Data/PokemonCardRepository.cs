using System.Collections.ObjectModel;
using PokemonCardCollection.Models;
using PokemonCardCollection.Services;
using PokemonCardCollection.Data.DTOs;

namespace PokemonCardCollection.Data;

/// <summary>
/// Repository that manages the in-memory collection of PokemonCards and coordinates API calls.
/// </summary>
public class PokemonCardRepository
{
    private readonly PokemonApiService _apiService;
    
    public ObservableCollection<PokemonCard> Cards { get; } = new();

    public PokemonCardRepository(PokemonApiService apiService)
    {
        _apiService = apiService;
    }

    /// <summary>
    /// Fetches initial cards from the API and populates the ObservableCollection.
    /// </summary>
    public async Task<string?> InitializeAsync()
    {
        if (Cards.Any()) return null;

        var (cards, error) = await _apiService.FetchInitialCardsAsync();
        
        if (error != null)
        {
            return error;
        }

        foreach (var card in cards)
        {
            Cards.Add(card);
        }

        return null;
    }

    /// <summary>
    /// Searches the API for cards matching the query.
    /// </summary>
    public async Task<List<TcgCardDto>> SearchCardsAsync(string query)
    {
        return await _apiService.SearchCardsAsync(query);
    }

    /// <summary>
    /// Fetches the full details of a specific card.
    /// </summary>
    public async Task<PokemonCard?> GetFullCardAsync(string id, string baseImageUri)
    {
        var (card, _) = await _apiService.GetFullCardAsync(id, baseImageUri);
        return card;
    }

    /// <summary>Returns a single card by its Id, or null.</summary>
    public PokemonCard? GetById(string id) => Cards.FirstOrDefault(c => c.Id == id);

    /// <summary>Adds a new card to the memory collection.</summary>
    public void Add(PokemonCard card)
    {
        if (string.IsNullOrEmpty(card.Id))
        {
            card.Id = Guid.NewGuid().ToString();
        }
        Cards.Add(card);
    }

    /// <summary>Updates an existing card's data in the memory collection.</summary>
    public void Update(PokemonCard card)
    {
        var index = Cards.ToList().FindIndex(c => c.Id == card.Id);
        if (index >= 0)
        {
            Cards[index] = card;
        }
    }

    /// <summary>Removes a card from the memory collection by Id.</summary>
    public void Delete(string id)
    {
        var card = Cards.FirstOrDefault(c => c.Id == id);
        if (card is not null)
        {
            Cards.Remove(card);
        }
    }

    /// <summary>Toggles the IsFavorite flag on a card.</summary>
    public void ToggleFavorite(string id)
    {
        var card = Cards.FirstOrDefault(c => c.Id == id);
        if (card is not null)
        {
            card.IsFavorite = !card.IsFavorite;
            Update(card);
        }
    }
}
