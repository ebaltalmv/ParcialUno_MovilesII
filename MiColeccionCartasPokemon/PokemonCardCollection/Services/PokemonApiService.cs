using System.Net.Http.Json;
using System.Text.Json;
using PokemonCardCollection.Data.DTOs;
using PokemonCardCollection.Models;

namespace PokemonCardCollection.Services;

public class PokemonApiService
{
    private readonly HttpClient _http;

    public PokemonApiService()
    {
        _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(8)
        };
    }

    /// <summary>
    /// Fetches a list of initial cards from the API and maps them to PokemonCard objects.
    /// </summary>
    public async Task<(List<PokemonCard> cards, string? error)> FetchInitialCardsAsync()
    {
        try
        {
            var dtos = await _http.GetFromJsonAsync<List<TcgCardDto>>("https://api.tcgdex.net/v2/en/cards?pagination:page=1&pagination:itemsPerPage=50");
            var resultList = new List<PokemonCard>();

            if (dtos != null)
            {
                int addedCount = 0;
                foreach (var d in dtos)
                {
                    if (addedCount >= 15) break;
                    if (string.IsNullOrEmpty(d.Image) || d.Name == "Unown" || string.IsNullOrEmpty(d.Id)) continue;

                    var (fullCard, _) = await GetFullCardAsync(d.Id, d.Image);
                    if (fullCard != null)
                    {
                        resultList.Add(fullCard);
                        addedCount++;
                    }
                }
            }

            return (resultList, null);
        }
        catch (TaskCanceledException)
        {
            return (new List<PokemonCard>(), "La petición tardó demasiado tiempo (timeout). Verifica tu conexión e intenta de nuevo.");
        }
        catch (HttpRequestException)
        {
            return (new List<PokemonCard>(), "No se pudo conectar al servidor o la ruta no existe.");
        }
        catch (JsonException)
        {
            return (new List<PokemonCard>(), "La respuesta del servidor no se pudo interpretar como JSON.");
        }
        catch (Exception ex)
        {
            return (new List<PokemonCard>(), $"Ocurrió un error inesperado: {ex.Message}");
        }
    }

    /// <summary>
    /// Searches the API for cards matching the query.
    /// </summary>
    public async Task<List<TcgCardDto>> SearchCardsAsync(string query)
    {
        try
        {
            var url = $"https://api.tcgdex.net/v2/en/cards?name={Uri.EscapeDataString(query)}";
            var dtos = await _http.GetFromJsonAsync<List<TcgCardDto>>(url);
            if (dtos != null)
            {
                return dtos.Where(d => !string.IsNullOrEmpty(d.Image) && d.Name != "Unown").Take(30).ToList();
            }
        }
        catch
        {
            // Ignore errors during search
        }
        return new List<TcgCardDto>();
    }

    /// <summary>
    /// Fetches the full details of a specific card by its ID.
    /// </summary>
    public async Task<(PokemonCard? card, string? error)> GetFullCardAsync(string id, string baseImageUri)
    {
        try
        {
            string imgUrl = $"{baseImageUri}/low.webp";
            var imgResponse = await _http.GetAsync(imgUrl, HttpCompletionOption.ResponseHeadersRead);
            if (!imgResponse.IsSuccessStatusCode) return (null, "Image not found");

            var fullCard = await _http.GetFromJsonAsync<TcgCardFullDto>($"https://api.tcgdex.net/v2/en/cards/{id}");
            if (fullCard == null) return (null, "Card details not found");

            decimal parsedPrice = 0m;
            var tcg = fullCard.Pricing?.TcgPlayer;
            if (tcg != null)
            {
                parsedPrice = tcg.Normal?.MarketPrice 
                    ?? tcg.Holofoil?.MarketPrice 
                    ?? tcg.ReverseHolofoil?.MarketPrice 
                    ?? 0m;
            }

            var card = new PokemonCard
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
            };

            return (card, null);
        }
        catch (TaskCanceledException)
        {
            return (null, "Timeout");
        }
        catch (HttpRequestException)
        {
            return (null, "Connection error");
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }
}
