using System.Net.Http.Json;

namespace PokemonCardCollection.Services;

/// <summary>
/// Typed-client implementation that talks to the TCGdex REST API.
/// Registered via <c>AddHttpClient&lt;ITcgdexService, TcgdexService&gt;()</c>.
/// </summary>
public class TcgdexService : ITcgdexService
{
    private readonly HttpClient _http;

    public TcgdexService(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://api.tcgdex.net/v2/es/");
    }

    /// <inheritdoc/>
    public async Task<List<TcgdexCardBriefDto>> GetCardsAsync(
        int page = 1,
        int itemsPerPage = 30,
        string? name = null,
        string? type = null,
        string? rarity = null)
    {
        var url = $"cards?pagination:page={page}&pagination:itemsPerPage={itemsPerPage}";

        if (!string.IsNullOrWhiteSpace(name))
        {
            url += $"&name={Uri.EscapeDataString(name)}";
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            url += $"&types={Uri.EscapeDataString(type)}";
        }

        if (!string.IsNullOrWhiteSpace(rarity))
        {
            url += $"&rarity={Uri.EscapeDataString(rarity)}";
        }

        var result = await _http.GetFromJsonAsync<List<TcgdexCardBriefDto>>(url);
        return result ?? [];
    }

    /// <inheritdoc/>
    public async Task<TcgdexCardDto?> GetCardByIdAsync(string cardId)
    {
        return await _http.GetFromJsonAsync<TcgdexCardDto>($"cards/{cardId}");
    }

    /// <inheritdoc/>
    public async Task<List<string>> GetTypesAsync()
    {
        var result = await _http.GetFromJsonAsync<List<string>>("types");
        return result ?? [];
    }

    /// <inheritdoc/>
    public async Task<List<string>> GetRaritiesAsync()
    {
        var result = await _http.GetFromJsonAsync<List<string>>("rarities");
        return result ?? [];
    }
}
