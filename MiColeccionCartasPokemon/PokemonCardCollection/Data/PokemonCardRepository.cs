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
            var dtos = await _http.GetFromJsonAsync<List<TcgCardDto>>("https://api.tcgdex.net/v2/en/cards?pagination:page=1&pagination:itemsPerPage=50");

            if (dtos != null)
            {
                int addedCount = 0;
                foreach (var d in dtos)
                {
                    if (addedCount >= 15) break;
                    if (string.IsNullOrEmpty(d.Image) || d.Name == "Unown") continue;

                    string imgUrl = $"{d.Image}/low.webp";
                    try
                    {
                        var imgResponse = await _http.GetAsync(imgUrl, HttpCompletionOption.ResponseHeadersRead);
                        if (!imgResponse.IsSuccessStatusCode) continue;

                        var fullCard = await _http.GetFromJsonAsync<TcgCardFullDto>($"https://api.tcgdex.net/v2/en/cards/{d.Id}");
                        if (fullCard == null) continue;

                        decimal parsedPrice = 0m;
                        var tcg = fullCard.Pricing?.TcgPlayer;
                        if (tcg != null)
                        {
                            parsedPrice = tcg.Normal?.MarketPrice 
                                ?? tcg.Holofoil?.MarketPrice 
                                ?? tcg.ReverseHolofoil?.MarketPrice 
                                ?? 0m;
                        }

                        Cards.Add(new PokemonCard
                        {
                            Id = fullCard.Id ?? Guid.NewGuid().ToString(),
                            Name = fullCard.Name ?? "Unknown",
                            ImageUri = imgUrl,
                            Category = fullCard.Types?.FirstOrDefault() ?? fullCard.Category ?? "Unknown",
                            Rarity = fullCard.Rarity ?? "Common",
                            Condition = "Mint",
                            EstimatedValue = parsedPrice,
                            IsFavorite = false,
                            Description = string.IsNullOrEmpty(fullCard.Description) ? "No description available." : fullCard.Description
                        });
                        addedCount++;
                    }
                    catch
                    {
                        // Ignore individual card fetch failures
                    }
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

public class TcgCardFullDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("rarity")]
    public string? Rarity { get; set; }

    [JsonPropertyName("types")]
    public List<string>? Types { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("pricing")]
    public PricingDto? Pricing { get; set; }
}

public class PricingDto
{
    [JsonPropertyName("tcgplayer")]
    public TcgPlayerDto? TcgPlayer { get; set; }
}

public class TcgPlayerDto
{
    [JsonPropertyName("normal")]
    public TcgPlayerVariantDto? Normal { get; set; }

    [JsonPropertyName("holofoil")]
    public TcgPlayerVariantDto? Holofoil { get; set; }

    [JsonPropertyName("reverse-holofoil")]
    public TcgPlayerVariantDto? ReverseHolofoil { get; set; }
}

public class TcgPlayerVariantDto
{
    [JsonPropertyName("marketPrice")]
    public decimal? MarketPrice { get; set; }
}
