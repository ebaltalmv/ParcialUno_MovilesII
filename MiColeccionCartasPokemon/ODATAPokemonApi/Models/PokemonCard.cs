namespace ODATAPokemonApi.Models;

/// <summary>
/// Representa una carta coleccionable de Pokémon.
/// </summary>
public class PokemonCard
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;       // Ej. Fire, Water, Grass, Electric, Psychic, Dragon
    
    public string Rarity { get; set; } = string.Empty;         // Ej. Common, Uncommon, Rare Holo, Ultra Rare, Secret Rare
    
    public string Condition { get; set; } = string.Empty;      // Ej. Mint, Near Mint, Played
    
    public decimal EstimatedValue { get; set; }
    
    public string ImageUri { get; set; } = string.Empty;       // URL o ruta de la imagen
    
    public bool IsFavorite { get; set; }
    
    public string Description { get; set; } = string.Empty;

    // Navigation property (Relación 1 a N con CardGrading)
    public ICollection<CardGrading> Gradings { get; set; } = new List<CardGrading>();
}
