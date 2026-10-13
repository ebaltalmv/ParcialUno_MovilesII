namespace PokemonCardCollection.Data.DTOs;

/// <summary>
/// Data Transfer Object que representa una certificación o graduación de una carta Pokémon
/// realizada por compañías especializadas como PSA, Beckett (BGS), CGC o SGC.
/// </summary>
public class CardGradingDto
{
    /// <summary>
    /// Identificador único de la certificación.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identificador foráneo de la carta Pokémon certificada.
    /// </summary>
    public int PokemonCardId { get; set; }

    /// <summary>
    /// Nombre de la empresa certificadora (ej. PSA, Beckett, CGC, SGC).
    /// </summary>
    public string GraderCompany { get; set; } = string.Empty;

    /// <summary>
    /// Calificación numérica otorgada a la condición física de la carta (escala 1 a 10).
    /// </summary>
    public decimal Score { get; set; }

    /// <summary>
    /// Fecha en que se emitió el certificado de autenticidad y condición.
    /// </summary>
    public DateTime CertifiedDate { get; set; }

    /// <summary>
    /// Código o número de serie único del certificado (ej. "PSA-94821038").
    /// </summary>
    public string CertificateCode { get; set; } = string.Empty;
}
