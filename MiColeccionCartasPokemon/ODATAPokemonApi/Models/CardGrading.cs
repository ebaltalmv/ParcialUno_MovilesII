using System.Text.Json.Serialization;

namespace ODATAPokemonApi.Models;

/// <summary>
/// Representa el certificado de graduación y autenticación de una carta Pokémon (PSA, Beckett, CGC, etc.).
/// </summary>
public class CardGrading
{
    public int Id { get; set; }
    
    public int PokemonCardId { get; set; }
    
    public string GraderCompany { get; set; } = string.Empty; // PSA, Beckett, CGC, SGC
    
    public decimal Score { get; set; } // Escala de 1 a 10 (ej. 9.5, 10.0)
    
    public DateTime CertifiedDate { get; set; }
    
    public string CertificateCode { get; set; } = string.Empty; // Ej. "PSA-94821038"

    // Navigation property (Relación N a 1 con PokemonCard)
    [JsonIgnore]
    public PokemonCard? PokemonCard { get; set; }
}
