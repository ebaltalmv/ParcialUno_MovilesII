using System.Collections.ObjectModel;
using System.Text.Json;
using PokemonCardCollection.Models;
using PokemonCardCollection.Services;

namespace PokemonCardCollection.Data;

/// <summary>
/// In-memory repository backed by an initial load from the TCGdex API.
/// All CRUD operations work against the local ObservableCollection — the API is read-only.
/// </summary>
public class PokemonCardRepository
{
    private readonly ITcgdexService _api;
    private bool _loaded;
    private int _nextId = 1;

    /// <summary>Observable collection of cards shared with the UI.</summary>
    public ObservableCollection<PokemonCard> Cards { get; } = new();

    /// <summary>True while an API fetch is in progress.</summary>
    public bool IsLoading { get; private set; }

    /// <summary>True if the last load attempt failed.</summary>
    public bool HasError { get; private set; }

    /// <summary>Human-readable error message (empty when no error).</summary>
    public string ErrorMessage { get; private set; } = string.Empty;

    public PokemonCardRepository(ITcgdexService api)
    {
        _api = api;
    }

    // ───────────────────────────── Initial load ─────────────────────────────

    /// <summary>
    /// Loads cards from the API on first call; subsequent calls are no-ops.
    /// </summary>
    public async Task EnsureLoadedAsync()
    {
        if (_loaded) return;

        await LoadCardsFromApiAsync(1, null, null, null);
    }

    /// <summary>
    /// Replaces the in-memory collection with one page of API results.
    /// Manual cards and cards from previous pages are removed by design.
    /// </summary>
    public async Task<int> LoadCardsFromApiAsync(int page, string? name, string? type, string? rarity)
    {
        Cards.Clear();

        IsLoading = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            var dtos = await _api.GetCardsAsync(page, 30, name, type, rarity);

            foreach (var dto in dtos)
            {
                Cards.Add(new PokemonCard
                {
                    Id = _nextId++,
                    ApiId = dto.Id,
                    Name = dto.Name,
                    Category = "Pokemon",
                    Rarity = "Por confirmar",
                    Condition = "Sin especificar",
                    EstimatedValue = 0,
                    ImageUri = dto.Image is not null ? $"{dto.Image}/low.webp" : "dotnet_bot.png",
                    IsFavorite = false,
                    Description = string.Empty
                });
            }

            _loaded = true;
            return dtos.Count;
        }
        catch (TaskCanceledException)
        {
            HasError = true;
            ErrorMessage = "La solicitud tardó demasiado. Verifica tu conexión.";
        }
        catch (HttpRequestException)
        {
            HasError = true;
            ErrorMessage = "No se pudo conectar con el servidor. Verifica tu conexión a internet.";
        }
        catch (JsonException)
        {
            HasError = true;
            ErrorMessage = "La respuesta del servidor no tiene el formato esperado.";
        }
        finally
        {
            IsLoading = false;
        }

        return 0;
    }

    // ──────────────────────────── CRUD (in-memory) ──────────────────────────

    /// <summary>Returns all cards in the collection.</summary>
    public List<PokemonCard> GetAll() => Cards.ToList();

    /// <summary>Returns a single card by its Id, or null.</summary>
    public PokemonCard? GetById(int id) => Cards.FirstOrDefault(c => c.Id == id);

    /// <summary>Returns only the cards marked as favorite.</summary>
    public List<PokemonCard> GetFavorites() => Cards.Where(c => c.IsFavorite).ToList();

    /// <summary>Adds a new card and assigns it a unique Id.</summary>
    public void Add(PokemonCard card)
    {
        card.Id = _nextId++;
        Cards.Add(card);
    }

    /// <summary>Updates an existing card's data (remove + insert to refresh bindings).</summary>
    public void Update(PokemonCard card)
    {
        var index = -1;
        for (var i = 0; i < Cards.Count; i++)
        {
            if (Cards[i].Id == card.Id) { index = i; break; }
        }
        if (index >= 0)
        {
            Cards.RemoveAt(index);
            Cards.Insert(index, card);
        }
    }

    /// <summary>Removes a card by Id.</summary>
    public void Delete(int id)
    {
        var card = Cards.FirstOrDefault(c => c.Id == id);
        if (card is not null)
            Cards.Remove(card);
    }

    /// <summary>Toggles the IsFavorite flag on a card.</summary>
    public void ToggleFavorite(int id)
    {
        var card = Cards.FirstOrDefault(c => c.Id == id);
        if (card is not null)
            card.IsFavorite = !card.IsFavorite;
    }

    // ───────────────────────── On-demand API detail ─────────────────────────

    /// <summary>
    /// Fetches full card details from TCGdex and enriches the local card.
    /// Called only from the Detail page to avoid saturating the API.
    /// </summary>
    public async Task<TcgdexCardDto?> GetCardDetailFromApiAsync(string apiId)
    {
        try
        {
            return await _api.GetCardByIdAsync(apiId);
        }
        catch (TaskCanceledException)
        {
            // Silently fail — detail enrichment is best-effort
            return null;
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }
}
