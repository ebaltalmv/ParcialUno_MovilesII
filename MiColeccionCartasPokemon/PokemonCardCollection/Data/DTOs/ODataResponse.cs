using System.Text.Json.Serialization;

namespace PokemonCardCollection.Data.DTOs;

/// <summary>
/// Envoltorio genérico estándar para deserializar respuestas de colecciones del protocolo OData v4.
/// Mapea las anotaciones de metadatos @odata y la lista de elementos en 'value'.
/// </summary>
/// <typeparam name="T">Tipo de dato o DTO de los elementos contenidos en el arreglo 'value'.</typeparam>
public class ODataResponse<T>
{
    /// <summary>
    /// URI de contexto de OData (@odata.context) que describe la metadata del conjunto devuelto.
    /// </summary>
    [JsonPropertyName("@odata.context")]
    public string? Context { get; set; }

    /// <summary>
    /// Cantidad total de registros devueltos o calculados por el servidor cuando se utiliza $count=true.
    /// </summary>
    [JsonPropertyName("@odata.count")]
    public int? Count { get; set; }

    /// <summary>
    /// Colección de registros de datos devueltos en la respuesta OData.
    /// </summary>
    [JsonPropertyName("value")]
    public List<T> Value { get; set; } = new();
}
