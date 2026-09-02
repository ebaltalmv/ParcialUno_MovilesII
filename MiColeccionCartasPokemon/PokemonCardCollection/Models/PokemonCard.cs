namespace PokemonCardCollection.Models;

/// <summary>
/// Represents a single Pokémon trading card in the collection.
/// </summary>
public class PokemonCard
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;       // e.g. Fire, Water, Grass, Electric…
    public string Rarity { get; set; } = string.Empty;         // e.g. Common, Uncommon, Rare, Ultra Rare
    public string Condition { get; set; } = string.Empty;      // e.g. Mint, Near Mint, Played
    public decimal EstimatedValue { get; set; }
    public string ImageUri { get; set; } = string.Empty;       // Local path or URL
    public bool IsFavorite { get; set; }
    public string Description { get; set; } = string.Empty;
}
