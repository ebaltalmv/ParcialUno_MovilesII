using System.Text.Json.Serialization;

namespace PokemonCardCollection.Data.DTOs;

public class TcgCardFullDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? Rarity { get; set; }
    public List<string>? Types { get; set; }
    public string? Description { get; set; }
    public PricingDto? Pricing { get; set; }
}

public class PricingDto
{
    public TcgPlayerDto? TcgPlayer { get; set; }
}

public class TcgPlayerDto
{
    public TcgPlayerVariantDto? Normal { get; set; }
    public TcgPlayerVariantDto? Holofoil { get; set; }
    
    [JsonPropertyName("reverse-holofoil")]
    public TcgPlayerVariantDto? ReverseHolofoil { get; set; }
}

public class TcgPlayerVariantDto
{
    public decimal? MarketPrice { get; set; }
}
