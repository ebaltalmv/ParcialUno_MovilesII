using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PokemonCardCollection.Data.DTOs;

namespace PokemonCardCollection.Services;

/// <summary>
/// Implementación del servicio cliente tipado (Typed Client) para interactuar con la API OData de Pokémon.
/// Gestiona la construcción de consultas OData ($expand, $filter), serialización y manejo de excepciones.
/// </summary>
public class PokemonCardService : IPokemonCardService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PokemonCardService>? _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public PokemonCardService(HttpClient httpClient, ILogger<PokemonCardService>? logger = null)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    /// <inheritdoc />
    public async Task<List<PokemonCardDto>> GetAllCardsAsync(string? filter = null)
    {
        try
        {
            var relativeUrl = "odata/PokemonCards?$expand=Gradings";

            if (!string.IsNullOrWhiteSpace(filter))
            {
                relativeUrl += $"&$filter={Uri.EscapeDataString(filter.Trim())}";
            }

            _logger?.LogInformation("Consultando cartas OData: {Url}", relativeUrl);

            var response = await _httpClient.GetFromJsonAsync<ODataResponse<PokemonCardDto>>(relativeUrl, _jsonOptions);

            return response?.Value ?? new List<PokemonCardDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Error HTTP al consultar cartas OData desde {BaseAddress}", _httpClient.BaseAddress);
            return new List<PokemonCardDto>();
        }
        catch (JsonException ex)
        {
            _logger?.LogError(ex, "Error de deserialización JSON de la respuesta OData");
            return new List<PokemonCardDto>();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error inesperado en GetAllCardsAsync");
            return new List<PokemonCardDto>();
        }
    }

    /// <inheritdoc />
    public async Task<PokemonCardDto?> GetCardByIdAsync(int id)
    {
        try
        {
            var relativeUrl = $"odata/PokemonCards({id})?$expand=Gradings";
            _logger?.LogInformation("Consultando carta con ID {Id} en OData: {Url}", id, relativeUrl);

            using var response = await _httpClient.GetAsync(relativeUrl);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger?.LogWarning("Carta con ID {Id} no encontrada (404)", id);
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PokemonCardDto>(_jsonOptions);
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Error HTTP al obtener la carta con ID {Id}", id);
            return null;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error inesperado al obtener la carta con ID {Id}", id);
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<PokemonCardDto?> CreateCardAsync(PokemonCardDto newCard)
    {
        try
        {
            const string relativeUrl = "odata/PokemonCards";
            _logger?.LogInformation("Enviando POST para registrar nueva carta: {CardName}", newCard.Name);

            using var response = await _httpClient.PostAsJsonAsync(relativeUrl, newCard, _jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger?.LogError("Error del servidor al crear la carta. Status: {StatusCode}, Detalle: {ErrorBody}", 
                    response.StatusCode, errorBody);
                return null;
            }

            var createdCard = await response.Content.ReadFromJsonAsync<PokemonCardDto>(_jsonOptions);
            _logger?.LogInformation("Carta creada exitosamente con ID {Id}", createdCard?.Id);
            return createdCard;
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "Error HTTP al registrar la nueva carta Pokémon");
            return null;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error inesperado al registrar la nueva carta Pokémon");
            return null;
        }
    }
}
