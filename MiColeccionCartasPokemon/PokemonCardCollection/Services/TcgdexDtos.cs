using System.Text.Json.Serialization;

namespace PokemonCardCollection.Services;

/// <summary>
/// Lightweight DTO returned by GET /cards (list endpoint).
/// Only a few fields are present; image may be null for some cards.
/// </summary>
public class TcgdexCardBriefDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("localId")]
    public string LocalId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string? Image { get; set; }
}

/// <summary>
/// Full DTO returned by GET /cards/{id} (detail endpoint).
/// </summary>
public class TcgdexCardDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }

    [JsonPropertyName("rarity")]
    public string? Rarity { get; set; }

    [JsonPropertyName("illustrator")]
    public string? Illustrator { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("types")]
    public List<string>? Types { get; set; }
}
