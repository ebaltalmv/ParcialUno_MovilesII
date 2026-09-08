namespace PokemonCardCollection.Services;

/// <summary>
/// Abstraction over the TCGdex REST API (read-only).
/// </summary>
public interface ITcgdexService
{
    /// <summary>Returns a page of card summaries, optionally filtered by name, type, or rarity.</summary>
    Task<List<TcgdexCardBriefDto>> GetCardsAsync(int page = 1, int itemsPerPage = 30, string? name = null, string? type = null, string? rarity = null);

    /// <summary>Returns full details for a single card.</summary>
    Task<TcgdexCardDto?> GetCardByIdAsync(string cardId);

    /// <summary>Returns the list of Pokémon types (e.g. "Agua", "Fuego").</summary>
    Task<List<string>> GetTypesAsync();

    /// <summary>Returns the list of card rarities (e.g. "Común", "Ultra Rara").</summary>
    Task<List<string>> GetRaritiesAsync();
}
