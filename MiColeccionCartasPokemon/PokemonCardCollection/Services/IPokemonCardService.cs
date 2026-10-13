using PokemonCardCollection.Data.DTOs;

namespace PokemonCardCollection.Services;

/// <summary>
/// Contrato del servicio cliente tipado para consumir la API OData de Cartas Pokémon.
/// </summary>
public interface IPokemonCardService
{
    /// <summary>
    /// Obtiene todas las cartas Pokémon registradas aplicando la expansión de sus certificaciones ($expand=Gradings)
    /// y opcionalmente un filtro OData ($filter).
    /// </summary>
    /// <param name="filter">Expresión de filtro OData opcional (ej. "EstimatedValue gt 50", "Category eq 'Fire'").</param>
    /// <returns>Colección de cartas con sus certificaciones asociadas.</returns>
    Task<List<PokemonCardDto>> GetAllCardsAsync(string? filter = null);

    /// <summary>
    /// Obtiene una carta Pokémon específica por su identificador primario, expandiendo sus certificaciones ($expand=Gradings).
    /// </summary>
    /// <param name="id">Identificador numérico de la carta.</param>
    /// <returns>La carta encontrada con sus Gradings o null si no se localizó.</returns>
    Task<PokemonCardDto?> GetCardByIdAsync(int id);

    /// <summary>
    /// Crea y persiste una nueva carta Pokémon mediante una petición HTTP POST en formato JSON al endpoint OData.
    /// </summary>
    /// <param name="newCard">Instancia con la información de la nueva carta.</param>
    /// <returns>La carta persistida con el identificador asignado por el servidor, o null si la operación falla.</returns>
    Task<PokemonCardDto?> CreateCardAsync(PokemonCardDto newCard);
}
