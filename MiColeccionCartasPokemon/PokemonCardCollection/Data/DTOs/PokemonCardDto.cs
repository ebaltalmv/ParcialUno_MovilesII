using System.Text.Json.Serialization;

namespace PokemonCardCollection.Data.DTOs;

/// <summary>
/// Data Transfer Object que representa una carta coleccionable de Pokémon, incluyendo su
/// relación 1 a N con certificaciones (Gradings) y propiedades de dominio calculadas.
/// </summary>
public class PokemonCardDto
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

    /// <summary>
    /// Relación 1 a N: Colección de certificaciones oficiales asociadas a la carta.
    /// Obtenida mediante la cláusula OData $expand=Gradings.
    /// </summary>
    public List<CardGradingDto> Gradings { get; set; } = new();

    // =========================================================================
    // PROPIEDADES CALCULADAS BASADAS EN LA COLECCIÓN RELACIONADA 'Gradings'
    // =========================================================================

    /// <summary>
    /// Calcula el promedio numérico de los scores otorgados por las entidades certificadoras.
    /// Retorna 0 si la carta no cuenta con certificaciones registradas.
    /// </summary>
    [JsonIgnore]
    public decimal PromedioGrading => Gradings is { Count: > 0 }
        ? Math.Round(Gradings.Average(g => g.Score), 2)
        : 0m;

    /// <summary>
    /// Conteo total de certificaciones registradas para esta carta Pokémon.
    /// </summary>
    [JsonIgnore]
    public int TotalGradings => Gradings?.Count ?? 0;

    /// <summary>
    /// Determina si la carta se considera valiosa o de alto interés coleccionista.
    /// Criterio: valor estimado >= $100 USD o promedio de grading >= 9.0.
    /// </summary>
    [JsonIgnore]
    public bool EsValiosa => EstimatedValue >= 100m || PromedioGrading >= 9.0m;
}
