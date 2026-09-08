using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using PokemonCardCollection.Models;

namespace PokemonCardCollection.Data;

/// <summary>
/// Repository that fetches data from TCGdex API and manages the in-memory collection.
/// </summary>
public class PokemonCardRepository
{
    private readonly HttpClient _http;
    
    public ObservableCollection<PokemonCard> Cards { get; } = new();

    public PokemonCardRepository()
    {
        _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(8)
        };
    }

    /// <summary>
    /// Fetches cards from the TCGdex API and populates the ObservableCollection.
    /// Handles loading errors internally, returning an error message if failed.
    /// </summary>
    public async Task<string?> InitializeAsync()
    {
        if (Cards.Any()) return null; // Already loaded

        try
        {
            var dtos = await _http.GetFromJsonAsync<List<TcgCardDto>>("https://api.tcgdex.net/v2/en/cards?limit=50");

            if (dtos != null)
            {
                var newCards = dtos.Take(50).Select(d => new PokemonCard
                {
                    Id = d.Id ?? Guid.NewGuid().ToString(),
                    Name = d.Name ?? "Unknown",
                    ImageUri = !string.IsNullOrEmpty(d.Image) ? $"{d.Image}/low.webp" : string.Empty,
                    Category = "Unknown",
                    Rarity = "Common",
                    Condition = "Mint",
                    EstimatedValue = 0m,
                    IsFavorite = false,
                    Description = "Loaded from TCGdex API"
                }).ToList();

                foreach (var card in newCards)
                {
                    Cards.Add(card);
                }
            }
            
            return null; // No errors
        }
        catch (TaskCanceledException)
        {
            return "La petición tardó demasiado tiempo (timeout). Verifica tu conexión e intenta de nuevo.";
        }
        catch (HttpRequestException)
        {
            return "No se pudo conectar al servidor o la ruta no existe.";
        }
        catch (JsonException)
        {
            return "La respuesta del servidor no se pudo interpretar como JSON.";
        }
        catch (Exception ex)
        {
            return $"Ocurrió un error inesperado: {ex.Message}";
        }
    }

    /// <summary>Returns a single card by its Id, or null.</summary>
    public PokemonCard? GetById(string id) => Cards.FirstOrDefault(c => c.Id == id);

    /// <summary>Adds a new card and assigns it a unique Id if not present.</summary>
    public void Add(PokemonCard card)
    {
        if (string.IsNullOrEmpty(card.Id))
        {
            card.Id = Guid.NewGuid().ToString();
        }
        Cards.Add(card);
    }

    /// <summary>Updates an existing card's data.</summary>
    public void Update(PokemonCard card)
    {
        var index = Cards.ToList().FindIndex(c => c.Id == card.Id);
        if (index >= 0)
        {
            Cards[index] = card;
        }
    }

    /// <summary>Removes a card by Id.</summary>
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
            // Trigger a re-assignment to update bindings if necessary, 
            // though ObservableObject properties inside card usually handle it.
            // But since Cards is ObservableCollection, replacing it triggers collection change.
            Update(card);
        }
    }
}

public class TcgCardDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }
}
